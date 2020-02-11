<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Rpt_Pat_Siniestros_Por_Vehiculo.aspx.cs" Inherits="paginas_Compras_Rpt_Pat_Siniestros_Por_Vehiculo" Title="Listado de Siniestros" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <script type="text/javascript" language="javascript">
        //Metodos para limpiar los controles de la busqueda.
        function Limpiar_Ctlr_Resguardante(){
            document.getElementById("<%=Txt_Resguardante.ClientID%>").value="";   
            document.getElementById("<%=Hdf_Resguardante_ID.ClientID%>").value ="";                      
            return false;
        }  
        function Limpiar_Ctlr_Busqueda_Resguardante(){
            document.getElementById("<%=Txt_Busqueda_No_Empleado.ClientID%>").value="";
            document.getElementById("<%=Txt_Busqueda_RFC.ClientID%>").value="";  
            document.getElementById("<%=Cmb_Busqueda_Dependencia.ClientID%>").value="";  
            document.getElementById("<%=Txt_Busqueda_Nombre_Empleado.ClientID%>").value="";                            
            return false;
        } 
        function Limpiar_Ctlr_Campos(){
            document.getElementById("<%=Chk_Reporte_Completo.ClientID%>").checked=false;
            document.getElementById("<%=Cmb_Mpio_Responsable.ClientID%>").value="";
            document.getElementById("<%=Cmb_Consignado.ClientID%>").value="";
            document.getElementById("<%=Cmb_Pago_Danio_Sindicos.ClientID%>").value="";
            document.getElementById("<%=Txt_No_Inventario.ClientID%>").value="";
            document.getElementById("<%=Cmb_Tipos_Vehiculo.ClientID%>").value="";  
            document.getElementById("<%=Txt_Fecha_Inicial.ClientID%>").value="";  
            document.getElementById("<%=Txt_Fecha_Final.ClientID%>").value="";  
            document.getElementById("<%=Cmb_Dependencia.ClientID%>").value="";  
            document.getElementById("<%=Cmb_Reparacion.ClientID%>").value="";  
            document.getElementById("<%=Cmb_Estatus.ClientID%>").value="";   
            document.getElementById("<%=Cmb_Tipo_Siniestros.ClientID%>").value="";  
            document.getElementById("<%=Cmb_Aseguradora.ClientID%>").value="";  
            Limpiar_Ctlr_Resguardante();
            Limpiar_Ctlr_Busqueda_Resguardante();                
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
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">              
                    <tr align="center">
                        <td class="label_titulo" colspan="2">Listado de Siniestros</td>
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
                            <asp:ImageButton ID="Btn_Generar_Reporte_PDF" runat="server" OnClick="Btn_Generar_Reporte_PDF_Click" ToolTip="Generar Reporte (Pdf)" Width="24px" ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png" />
                            <asp:ImageButton ID="Btn_Generar_Reporte_Excel" runat="server" ToolTip="Generar Reporte (Excel)" Width="24px" ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png" onclick="Btn_Generar_Reporte_Excel_Click" />
                            <asp:ImageButton ID="Btn_Limpiar_Filtros" runat="server" ToolTip="Limpiar Filtros" Width="24px" ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png" OnClientClick="javascript:return Limpiar_Ctlr_Campos();" />
                        </td>
                        <td>&nbsp;</td>                        
                    </tr>
                </table>   
                <br />
                <center>
                    <table width="98%" border="0" cellspacing="0" class="estilo_fuente">
                        <tr>
                            <td style="width:18%">
                                <asp:Label ID="Lbl_Completo" runat="server" Text="COMPLETO" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:CheckBox ID="Chk_Reporte_Completo" runat="server"/>
                                <asp:Label ID="Lbl_Nota_Completo" runat="server" Text="Nota: El Reporte Completo incluye las observaciones realizadas para el siniestro." CssClass="estilo_fuente" style="font-size:xx-small; font-weight:bolder;"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4"><hr /></td>
                        </tr> 
                        <tr>
                            <td style="width:20%; text-align:left;">
                                <asp:Label ID="Lbl_Tipo_Siniestro" runat="server" Text="Tipo Siniestro" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width:30%; text-align:left;" colspan="3">
                                <asp:DropDownList ID="Cmb_Tipo_Siniestros" runat="server" Width="100%">
                                </asp:DropDownList>
                            </td>
                        </tr> 
                        <tr>
                            <td style="width:18%; text-align:left; ">
                                <asp:Label ID="Lbl_Fecha_Inicial" runat="server" Text="Fecha Inicial" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width:32%; text-align:left; ">
                                <asp:TextBox ID="Txt_Fecha_Inicial" runat="server" Width="85%" MaxLength="20" Enabled="false" ></asp:TextBox>
                                <asp:ImageButton ID="Btn_Fecha_Inicial" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                                <cc1:CalendarExtender ID="CE_Txt_Fecha_Inicial" runat="server" TargetControlID="Txt_Fecha_Inicial" PopupButtonID="Btn_Fecha_Inicial" Format="dd/MMM/yyyy">
                                </cc1:CalendarExtender>
                            </td>
                            <td style="width:18%; text-align:left; ">
                                <asp:Label ID="Lbl_Fecha_Final" runat="server" Text="Fecha Final" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width:32%; text-align:left; ">
                                <asp:TextBox ID="Txt_Fecha_Final" runat="server" Width="85%" MaxLength="20" Enabled="false" ></asp:TextBox>
                                <asp:ImageButton ID="Btn_Fecha_Final" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                                <cc1:CalendarExtender ID="CE_Txt_Fecha_Final" runat="server" TargetControlID="Txt_Fecha_Final" PopupButtonID="Btn_Fecha_Final" Format="dd/MMM/yyyy">
                                </cc1:CalendarExtender>
                            </td>
                        </tr>    
                        <tr>
                            <td style="width:18%; text-align:left; vertical-align:top;">
                               <asp:Label ID="Lbl_Reparacion" runat="server" Text="Reparación" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td  style="text-align:left;  width:32%">
                                <asp:DropDownList ID="Cmb_Reparacion" runat="server" Width="100%">
                                    <asp:ListItem Value="">&lt; TODOS &gt;</asp:ListItem>
                                    <asp:ListItem Value="ASEGURADORA">ASEGURADORA</asp:ListItem>
                                    <asp:ListItem Value="TALLER_MUNICIPAL">TALLER MUNICIPAL</asp:ListItem>
                                    <asp:ListItem Value="SIN REPARACIÓN">SIN REPARACIÓN</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width:18%; text-align:left; vertical-align:top;">
                               <asp:Label ID="Lbl_Estatus" runat="server" Text="Estatus" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="text-align:left; width:32%">
                                <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="100%">
                                    <asp:ListItem Text ="&lt;TODOS&gt;" Value=""></asp:ListItem>
                                    <asp:ListItem Text ="PENDIENTE" Value="PENDIENTE"></asp:ListItem>
                                    <asp:ListItem Text ="BAJA" Value="BAJA"></asp:ListItem>
                                    <asp:ListItem Text ="FINALIZADO" Value="FINALIZADO"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr> 
                        <tr>
                            <td style="width:18%; text-align:left; vertical-align:top;">
                               <asp:Label ID="Lbl_Responsable_Municipio" runat="server" Text="Mpio. Responsable" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="text-align:left; width:32%;">
                                <asp:DropDownList ID="Cmb_Mpio_Responsable" runat="server" Width="100%">
                                    <asp:ListItem Text ="&lt;TODOS&gt;" Value=""></asp:ListItem>
                                    <asp:ListItem Text ="SI" Value="SI"></asp:ListItem>
                                    <asp:ListItem Text ="NO" Value="NO"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width:18%; text-align:left; vertical-align:top;">
                               <asp:Label ID="Lbl_Consignado" runat="server" Text="Consignado" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="text-align:left; width:32%;">
                                <asp:DropDownList ID="Cmb_Consignado" runat="server" Width="100%">
                                    <asp:ListItem Text ="&lt;TODOS&gt;" Value=""></asp:ListItem>
                                    <asp:ListItem Text ="SI" Value="SI"></asp:ListItem>
                                    <asp:ListItem Text ="NO" Value="NO"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>   
                        <tr>
                            <td style="width:18%; text-align:left; vertical-align:top;">
                               <asp:Label ID="Lbl_Pago_Danios_Sindicos" runat="server" Text="Pago a Sindicos" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="text-align:left; width:32%;">
                                <asp:DropDownList ID="Cmb_Pago_Danio_Sindicos" runat="server" Width="100%">
                                    <asp:ListItem Text ="&lt;TODOS&gt;" Value=""></asp:ListItem>
                                    <asp:ListItem Text ="SI" Value="SI"></asp:ListItem>
                                    <asp:ListItem Text ="NO" Value="NO"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>  
                        <tr>
                            <td colspan="4"><hr /></td>
                        </tr> 
                        <tr>
                            <td style="width:18%">
                                <asp:Label ID="Lbl_No_Inventario" runat="server" Text="No. Inventario" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width:32%">
                                <asp:TextBox ID="Txt_No_Inventario" runat="server" Width="97%"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_No_Inventario" runat="server" TargetControlID="Txt_No_Inventario" FilterType="Numbers">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                            <td style="width:18%">
                                 <asp:Label ID="Lbl_Tipos_Vehiculo" runat="server" Text="Tipo de Vehículo" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width:32%">
                                <asp:DropDownList ID="Cmb_Tipos_Vehiculo" runat="server" Width="98%">
                                    <asp:ListItem>&lt; -- SELECCIONE -- &gt;</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>    
                        <tr>
                            <td style="width:20%; text-align:left;">
                                <asp:Label ID="Lbl_Aseguradora" runat="server" Text="Aseguradora" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width:30%; text-align:left;" colspan="3">
                                <asp:DropDownList ID="Cmb_Aseguradora" runat="server" Width="100%">
                                </asp:DropDownList>
                            </td>
                        </tr>         
                        <tr>
                            <td style="width:18%; text-align:left; ">
                                <asp:Label ID="Lbl_Dependencias" runat="server" Text="Unidad Responsable" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:DropDownList ID="Cmb_Dependencia" runat="server" Width="99%">
                                    <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>   
                                </asp:DropDownList>                                   
                            </td>
                        </tr>                                                                   
                        <tr>
                            <td style="width:18%; text-align:left; ">
                                <asp:HiddenField ID="Hdf_Resguardante_ID" runat="server" />
                                <asp:Label ID="Lbl_Resguardante" runat="server" Text="Resguardante" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td colspan="3" style="text-align:left;">
                                <asp:ImageButton ID="Btn_Limpiar_Resguardante" runat="server" ToolTip="Limpiar Resguardante" AlternateText="Limpiar Resguardante" ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png" Width="16px"  OnClientClick="javascript:return Limpiar_Ctlr_Resguardante();" />
                                &nbsp;
                                <asp:TextBox ID="Txt_Resguardante" runat="server" Width="90%" Enabled="false" ></asp:TextBox>  
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Resguardante" runat="server" TargetControlID ="Txt_Resguardante" WatermarkText="<<<< TODOS >>>>" WatermarkCssClass="watermarked"/>                                                                                
                                <asp:ImageButton ID="Btn_Busqueda_Avanzada_Resguardante" runat="server" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Width="16px" ToolTip="Buscar" AlternateText="Buscar" OnClick="Btn_Busqueda_Avanzada_Resguardante_Click"/>
                            </td>
                        </tr>   
                    </table>
                </center>
                <br />                           
            </div>
            <br />
            <br />
            <br />
            <br />
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
                                            <asp:ImageButton ID="Btn_Limpiar_Ctlr_Busqueda" runat="server" OnClientClick="javascript:return Limpiar_Ctlr_Busqueda_Resguardante();"
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
                                               <asp:Button ID="Btn_Busqueda_Empleados" runat="server"  Text="Busqueda de Empleados" CssClass="button" CausesValidation="false"  Width="200px" OnClick="Btn_Busqueda_Empleados_Click" /> 
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


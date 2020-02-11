<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Rpt_Pat_Com_Licencias_Vencidas.aspx.cs" Inherits="paginas_predial_Frm_Rpt_Pat_Com_Licencias_Vencidas" Title="Reporte de Licencias Vencidas" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script type="text/javascript" language="javascript">
        //Metodos para limpiar los controles de la busqueda.
        function Limpiar_Ctlr_Busqueda_Resguardante(){
            document.getElementById("<%=Txt_Busqueda_No_Empleado.ClientID%>").value="";
            document.getElementById("<%=Txt_Busqueda_RFC.ClientID%>").value="";  
            document.getElementById("<%=Cmb_Busqueda_Dependencia.ClientID%>").value="";  
            document.getElementById("<%=Txt_Busqueda_Nombre_Empleado.ClientID%>").value="";                            
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
                        <td class="label_titulo">Reporte de Licencias</td>
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
                                <td style="width:90%;text-align:left;" valign="top" >
                                  <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text=""  CssClass="estilo_fuente_mensaje_error" />
                                </td>
                              </tr>          
                            </table>                   
                          </div>                          
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>                        
                    </tr>
                    <tr class="barra_busqueda" align="left">
                        <td>
                            &nbsp;&nbsp;
                            <asp:ImageButton ID="Btn_Salir" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" Width="24px" CssClass="Img_Button"
                                             AlternateText="Salir" OnClick="Btn_Salir_Click"/>
                        </td>                        
                    </tr>
                </table>   
                <br />
                <center>
                    <div style="border-width:medium; width:95%; border-bottom-style:solid;" >
                    <table width="100%">                               
                        <tr>
                            <td style="width:20%; text-align:left; ">
                                <asp:Label ID="Lbl_Busqueda_RFC_Empleado" runat="server" Text="RFC" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width:30%; text-align:left;">
                                <asp:TextBox ID="Txt_Busqueda_RFC_Empleado" runat="server" Width="96%"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_RFC_Empleado" runat="server" TargetControlID="Txt_Busqueda_RFC_Empleado" InvalidChars="<,>,&,',!,"  FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True">        
                                </cc1:FilteredTextBoxExtender>  
                            </td>
                            <td style="width:20%; text-align:left; ">
                                <asp:Label ID="Lbl_Busqueda_No_Empleado" runat="server" Text="No. Empleado" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width:30%; text-align:left;">
                                <asp:TextBox ID="Txt_Busqueda_No_Empleado" runat="server" Width="96%"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_No_Empleado" runat="server" TargetControlID="Txt_Busqueda_No_Empleado" InvalidChars="<,>,&,',!,"  FilterType="Numbers"  Enabled="True">        
                                </cc1:FilteredTextBoxExtender>  
                            </td>
                        </tr>                          
                        <tr>
                            <td style="width:20%; text-align:left; ">
                                <asp:Label ID="Lbl_Busqueda_Dependencias" runat="server" Text="Unidad Responsable" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td colspan="3" style="text-align:left;">
                                <asp:DropDownList ID="Cmb_Busqueda_Dependencias" runat="server" Width="99%" AutoPostBack="true" OnSelectedIndexChanged="Cmb_Busqueda_Dependencias_SelectedIndexChanged">
                                    <asp:ListItem Text="&lt;-- TODAS --&gt;" Value="TODAS"></asp:ListItem>   
                                </asp:DropDownList>                                   
                            </td>
                        </tr>                          
                        <tr>
                            <td style="width:20%; text-align:left; ">
                                <asp:Label ID="Lbl_Busqueda_Nombre_Empleado" runat="server" Text="Nombre Resguardante" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width:80%; text-align:left;" colspan="3">
                                <asp:DropDownList ID="Cmb_Busqueda_Nombre_Empleado" runat="server" Width="90%" >
                                    <asp:ListItem Text="&lt;-- HACE FALTA SELECCIONAR UNA UNIDAD RESPONSABLE --&gt;" Value="SELECCIONE"></asp:ListItem>
                                </asp:DropDownList> 
                                <asp:ImageButton ID="Btn_Busqueda_Avanzada_Resguardante" runat="server" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Width="16px" ToolTip="Buscar Empleado" AlternateText="Buscar Empleado" OnClick="Btn_Busqueda_Avanzada_Resguardante_Click"/>
                            </td>
                        </tr>                                  
                        <tr>
                            <td style="text-align:left; width:20%">
                                <asp:Label ID="Lbl_Fecha_Desde" runat="server" Text="Fecha Desde"  CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="text-align:left; width:30%">
                                <asp:TextBox ID="Txt_Fecha_Desde" runat="server" Width="85%" MaxLength="20" Enabled="false"></asp:TextBox>
                                <asp:ImageButton ID="Btn_Fecha_Desde" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                                <cc1:CalendarExtender ID="CE_Txt_Fecha_Desde" runat="server" TargetControlID="Txt_Fecha_Desde" PopupButtonID="Btn_Fecha_Desde" Format="dd/MMM/yyyy">
                                </cc1:CalendarExtender>
                            </td>
                            <td style="text-align:left; width:20%">
                                <asp:Label ID="Lbl_Fecha_Hasta" runat="server" Text="Fecha Hasta"  CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="text-align:left; width:30%">
                                <asp:TextBox ID="Txt_Fecha_Hasta" runat="server" Width="80%" MaxLength="20" Enabled="false"></asp:TextBox>
                                <asp:ImageButton ID="Btn_Fecha_Hasta" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                                <cc1:CalendarExtender ID="CE_Txt_Fecha_Hasta" runat="server" TargetControlID="Txt_Fecha_Hasta" PopupButtonID="Btn_Fecha_Hasta" Format="dd/MMM/yyyy">
                                </cc1:CalendarExtender>
                            </td>
                        </tr>    
                        <tr>
                            <td style="text-align:left; " colspan="4">
                                <hr />
                            </td>
                        </tr>                                                                                                                   
                        <tr>
                            <td style="text-align:left; " colspan="3">
                                <asp:Label ID="Lbl_Con_Licencia" runat="server" Text="Elegir" CssClass="estilo_fuente"></asp:Label>
                                &nbsp;
                                <asp:DropDownList ID="Cmb_Con_Licencia" runat="server" Width="200px" >
                                    <asp:ListItem Text="&lt;-- TODOS --&gt;" Value="TODOS"></asp:ListItem>
                                    <asp:ListItem Text="CON LICENCIA" Value="CON_LICENCIA"></asp:ListItem>
                                    <asp:ListItem Text="SIN LICENCIA" Value="SIN_LICENCIA"></asp:ListItem>
                                </asp:DropDownList>     
                            </td>
                            <td style="text-align:right;">
                                <asp:ImageButton ID="Btn_Generar_Listado" runat="server" ImageUrl="~/paginas/imagenes/paginas/Listado.png" CausesValidation="False" 
                                    OnClick="Btn_Generar_Listado_Click" AlternateText="Generar Listado" ToolTip="Generar Listado" />
                                <asp:ImageButton ID="Btn_Generar_Reporte_PDF" runat="server" ToolTip="Generar Reporte (PDF)" ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png" Width="24px" CssClass="Img_Button" 
                                    AlternateText="Generar Reporte (PDF)" onclick="Btn_Generar_Reporte_PDF_Click" /> 
                                <asp:ImageButton ID="Btn_Generar_Reporte_Excel" runat="server" ToolTip="Generar Reporte (Excel)" ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png" Width="24px" CssClass="Img_Button" 
                                    AlternateText="Generar Reporte (Excel)" onclick="Btn_Generar_Reporte_Excel_Click" /> 
                                <asp:ImageButton ID="Btn_Limpiar_Filtros" runat="server" ToolTip="Limpiar Filtros" CausesValidation="False" ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png" 
                                    AlternateText="Limpiar Filtros" onclick="Btn_Limpiar_Filtros_Click" Width="25px"/>       
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;                               
                            </td>
                        </tr>                                                             
                    </table>
                </div>
                </center>
                <div style="width:98%">
                    <center>
                            <br />
                            <asp:GridView ID="Grid_Listado_Empleados" runat="server" AllowPaging="True" 
                                AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                                PageSize="15" OnPageIndexChanging="Grid_Listado_Empleados_PageIndexChanging"
                                Width="98%">
                                <RowStyle CssClass="GridItem" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <Columns>
                                <asp:BoundField DataField="EMPLEADO_ID" HeaderText="EMPLEADO_ID" SortExpression="EMPLEADO_ID" />
                                <asp:BoundField DataField="NO_EMPLEADO" HeaderText="No. Empleado" SortExpression="NO_EMPLEADO" NullDisplayText="-" >
                                    <ItemStyle Width="90px" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="RFC" HeaderText="RFC" SortExpression="RFC" NullDisplayText="-">
                                    <ItemStyle Width="130px" HorizontalAlign="Center"/>
                                </asp:BoundField>
                                <asp:BoundField DataField="NOMBRE_COMPLETO" HeaderText="Nombre del Empleado" SortExpression="NOMBRE_COMPLETO" NullDisplayText="-"/>
                                <asp:BoundField DataField="NO_LICENCIA_MANEJO" HeaderText="No. Licencia" SortExpression="NO_LICENCIA_MANEJO" NullDisplayText="-">
                                    <ItemStyle Width="90px" HorizontalAlign="Center"/>
                                </asp:BoundField>
                                <asp:BoundField DataField="TIPO_LICENCIA" HeaderText="Tipo de Licencia" SortExpression="TIPO_LICENCIA" NullDisplayText="-">
                                    <ItemStyle Width="90px" HorizontalAlign="Center"/>
                                </asp:BoundField>
                                <asp:BoundField DataField="FECHA_VENCIMIENTO_LICENCIA" HeaderText="Vencimiento" SortExpression="FECHA_VENCIMIENTO_LICENCIA" DataFormatString="{0:dd/MMM/yyyy}" NullDisplayText="-">
                                    <ItemStyle Width="90px" HorizontalAlign="Center"/>
                                </asp:BoundField>
                                </Columns>
                                <HeaderStyle CssClass="GridHeader" />
                                <PagerStyle CssClass="GridHeader" />
                                <SelectedRowStyle CssClass="GridSelected" />
                            </asp:GridView>
                    </center>   
                </div>                                             
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
                                           <asp:TextBox ID="Txt_Buscar_Resguardante_No_Empleado" runat="server" Width="98%" />
                                           <cc1:FilteredTextBoxExtender ID="FTE_Txt_Buscar_Resguardante_No_Empleado" runat="server" FilterType="Numbers" TargetControlID="Txt_Buscar_Resguardante_No_Empleado"/>
                                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Buscar_Resguardante_No_Empleado" runat="server" TargetControlID ="Txt_Buscar_Resguardante_No_Empleado" WatermarkText="Busqueda por No Empleado" 
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
                                            OnPageIndexChanging="Grid_Busqueda_Empleados_Resguardo_PageIndexChanging">
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
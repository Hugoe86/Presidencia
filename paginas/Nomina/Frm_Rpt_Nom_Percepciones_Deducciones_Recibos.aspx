<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Rpt_Nom_Percepciones_Deducciones_Recibos.aspx.cs" Inherits="paginas_Nomina_Frm_Rpt_Nom_Percepciones_Deducciones_Recibos" Title="Reporte de Percepciones y Deducciones"%>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <script type="text/javascript" language="javascript">
        function refresh_tabla_empleados(){ $('#tabla_empleados table > tbody > tr').remove();}
        function Limpiar_Ctlr_Busqueda_Empleado(){
            document.getElementById("<%=Txt_Busqueda_No_Empleado.ClientID%>").value="";
            document.getElementById("<%=Txt_Busqueda_RFC.ClientID%>").value="";  
            document.getElementById("<%=Cmb_Busqueda_Dependencia.ClientID%>").value="";  
            document.getElementById("<%=Txt_Busqueda_Nombre_Empleado.ClientID%>").value="";                            
            return false;
        } 
        function Limpiar_Ctlr_Empleado(){
            document.getElementById("<%=Hdf_Empleado_ID.ClientID%>").value="";  
            document.getElementById("<%=Txt_No_Empleado.ClientID%>").value="";
            document.getElementById("<%=Txt_Nombre_Empleado.ClientID%>").value="";                            
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

            <div style="width:98%; background-color:White;">
                <table width="100%" title="Control_Errores"> 
                    <tr>
                        <td style="width:100%; text-align:center; cursor:default; font-size:14px;" class="estilo_fuente">
                            Reporte de Percepciones y Deducciones
                        </td>               
                    </tr>            
                    <tr>
                        <td style="width:100%; text-align:left; cursor:default;">
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"/>        
                        </td>               
                    </tr>
                </table>
                
                <table width="98%"  border="0" cellspacing="0">
                    <tr align="center">
                        <td>                
                            <div align="right" class="barra_busqueda">                        
                                <table style="width:100%;height:28px;">
                                    <tr>
                                        <td align="left" style="width:59%;">
                                            <asp:ImageButton ID="Btn_Reporte_Pdf" runat="server" ToolTip="Generar Reporte" 
                                                CssClass="Img_Button" 
                                                ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png" 
                                                onclick="Btn_Reporte_Pdf_Click"/>
                                            <asp:ImageButton ID="Btn_Reporte_Excel" runat="server" 
                                                 ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png"
                                                  CssClass="Img_Button"  AlternateText="Imprimir Excel" 
                                                 ToolTip="Exportar Excel"
                                                 OnClick="Btn_Reporte_Excel_Click" Visible="true"/> 
                                        </td>
                                        <td align="right" style="width:41%;">&nbsp;</td>       
                                    </tr>         
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
             </div>   
      <asp:UpdatePanel ID="UPnl_Rpt_Incidencias_Empleados" runat="server" UpdateMode="Always">
        <ContentTemplate>
    
        <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="UPnl_Rpt_Incidencias_Empleados" DisplayAfter="0">
            <ProgressTemplate>
                <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
            </ProgressTemplate>
        </asp:UpdateProgress>  
        
            <div style="width:98%; background-color:White;">
                <asp:Panel ID="Pnl_Tipo_Reporte" runat="server" Width="100%" GroupingText="Tipo de Reporte">  
                    <table width="100%"> 
                        <tr>
                            <td  class="button_autorizar" style="width:20%; text-align:left; cursor:default;">
                            Tipo
                            </td>

                            <td class="button_autorizar" style="width:80%; text-align:left; cursor:default;">
                                <asp:DropDownList ID="Cmb_Tipo_Reporte" runat="server" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="Cmb_Tipo_Reporte_SelectedIndexChanged" >
                                    <asp:ListItem Value ="DEDUCCION">REPORTE DE DEDUCCIONES</asp:ListItem>
                                    <asp:ListItem Value ="PERCEPCION">REPORTE DE PERCEPCIONES</asp:ListItem>
                                </asp:DropDownList>
                            </td> 
                        </tr>
                    </table>
                </asp:Panel> 
                    
                  <asp:Panel ID="Pnl_Filtros_Empleado" runat="server" Width="100%" GroupingText="Selección de Empleado">        
                    <table width="100%">              
                        <tr>
                            <td class="button_autorizar" style="width:20%; text-align:left; cursor:default;">
                                <asp:HiddenField ID="Hdf_Empleado_ID" runat="server" />
                                N&uacute;mero de Empleado
                            </td>
                            <td class="button_autorizar" style="width:30%; text-align:left; cursor:default;">
                                <asp:TextBox ID="Txt_No_Empleado" runat="server" Width="98%"  MaxLength="20" Enabled="false"/>
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_No_Empleado" runat="server" TargetControlID="Txt_No_Empleado" FilterType="Numbers" ValidChars="0123456789" />                        
                            </td>   
                            <td class="button_autorizar" style="width:10%; text-align:left; cursor:default;"> 
                                <asp:ImageButton ID="Btn_Buscar_Empleado" runat="server" ToolTip="Consultar"  ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png"  onclick="Btn_Buscar_Empleado_Click"  Width="24px"/>
                                <asp:ImageButton ID="Btn_Limpiar_Empleado" runat="server" ToolTip="Limpiar Empleado"  ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png"  Width="24px" OnClientClick="javascript:return Limpiar_Ctlr_Empleado();"/>
                            </td>
                            <td  style="width:50%; text-align:left; cursor:default;"></td>
                       </tr>  
                    </table>
                        
                    <table width="100%"> 
                         <tr>
                            <td class="button_autorizar" style="width:20%; text-align:left; cursor:default;"> Nombre</td>
                            <td class="button_autorizar" style="width:80%; text-align:left; cursor:default;">
                                <asp:TextBox ID="Txt_Nombre_Empleado" runat="server" Width="98%" Enabled="false" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                    
                <asp:Panel ID="Pnl_Dependencia" runat="server" Width="100%" GroupingText="">   
                    <table width="100%">  
                        <tr>   
                            <td class="button_autorizar" style="width:20%; text-align:left; cursor:default;">
                                U. Responsable
                            </td>
                            <td class="button_autorizar" style="width:80%; text-align:left; cursor:default;">
                                <asp:DropDownList ID="Cmb_Unidad_Responsable" runat="server" Width="100%"  OnChange="javascript:return Limpiar_Ctlr_Empleado();"/>
                            </td>             
                        </tr>  
                         
                    </table>
                </asp:Panel>
                    
                    
                <asp:Panel ID="Pnl_Tipo_Nomina" runat="server" Width="100%" GroupingText="Tipo de Nomina">   
                    <table width="100%"> 
                        <tr>
                            <td class="button_autorizar" style="width:20%; text-align:left; cursor:default;">
                                Tipo Nómina
                            </td>
                            <td class="button_autorizar" style="width:80%; text-align:left; cursor:default;">
                                <asp:DropDownList ID="Cmb_Tipo_Nomina" runat="server" Width="100%" />
                            </td> 
                        </tr> 
                      </table>
                </asp:Panel>
                    
                <asp:Panel ID="Pnl_Fechas" runat="server" Width="100%" GroupingText="Rangos de Fechas">  
                     <table width="100%"> 
                         <tr>
                            <td  class="button_autorizar" style="width:20%; text-align:left; cursor:default;">
                                Nomina
                            </td>
                            
                            <td class="button_autorizar" style="width:30%; text-align:left; cursor:default;">
                                <asp:DropDownList ID="Cmb_Calendario_Nomina" runat="server" Width="95%" AutoPostBack="true" 
                                     onselectedindexchanged="Cmb_Calendario_Nomina_SelectedIndexChanged"/>
                            </td> 
                            
                            <td  class="button_autorizar" style="width:20%; text-align:left; cursor:default;">
                                Periodo
                            </td>
                            
                            <td class="button_autorizar" style="width:30%; text-align:left; cursor:default;">
                                <asp:DropDownList ID="Cmb_Periodos_Catorcenales_Nomina" runat="server" Width="95%" 
                                    AutoPostBack="true" OnSelectedIndexChanged="Cmb_Periodos_Catorcenales_Nomina_SelectedIndexChanged"/>
                            </td> 
                        </tr>
                        <tr>
                            <td  class="button_autorizar" style="width:20%; text-align:left; cursor:default;">
                                Fecha Inicial
                            </td>
                            <td class="button_autorizar" style="width:30%; text-align:left; cursor:default;">
                                <asp:TextBox ID="Txt_Fecha_Inicial" runat="server" Width="98%"  MaxLength="8" ToolTip="ddmmaaaa"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Txt_Fecha_Inicial_FilteredTextBoxExtender" runat="server" TargetControlID="Txt_Fecha_Inicial" FilterType="Custom, Numbers"
                                ValidChars="0123456789" />
                            </td>
                            
                            <td  class="button_autorizar" style="width:20%; text-align:left; cursor:default;">
                                Fecha Final
                            </td>
                            <td class="button_autorizar" style="width:30%; text-align:left; cursor:default;">
                                <asp:TextBox ID="Txt_Fecha_Final" runat="server" Width="98%"  MaxLength="8" ToolTip="ddmmaaaa"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Txt_Fecha_Final_FilteredTextBoxExtender1" runat="server" TargetControlID="Txt_Fecha_Final" FilterType="Custom, Numbers"
                                ValidChars="0123456789" />
                            </td>
                       </tr>
                    </table>
                </asp:Panel>
                    
                <asp:Panel ID="Pnl_Concepto" runat="server" Width="100%" GroupingText="Tipo de Concepto">  
                    <table width="100%"> 
                        <tr>
                            <td  class="button_autorizar" style="width:20%; text-align:left; cursor:default;">
                                Concepto
                            </td>
                            <td  class="button_autorizar" style="width:30%; text-align:left; cursor:default;">
                                <asp:TextBox ID="Txt_Concepto" runat="server" Width="98%" style="text-transform: uppercase;" OnTextChanged="Txt_Concepto_TextChanged" AutoPostBack="true"></asp:TextBox>
                            </td>
                            <td  class="button_autorizar" style="width:60%; text-align:left; cursor:default;">
                                <asp:DropDownList ID="Cmb_Percepciones" runat="server" Width="98%"></asp:DropDownList>
                             </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
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
                                            <asp:ImageButton ID="Btn_Limpiar_Ctlr_Busqueda" runat="server" OnClientClick="javascript:return Limpiar_Ctlr_Busqueda_Empleado();"
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
                                      <asp:GridView ID="Grid_Busqueda_Empleados" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                            ForeColor="#333333" GridLines="None" AllowPaging="True" Width="100%" 
                                            PageSize="100" EmptyDataText="No se encontrarón resultados para los filtros de la busqueda" 
                                            OnSelectedIndexChanged="Grid_Busqueda_Empleados_SelectedIndexChanged"
                                            OnPageIndexChanging="Grid_Busqueda_Empleados_PageIndexChanging"
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


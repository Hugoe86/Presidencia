<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Rpt_Nom_Formato_Pago_Proveedores.aspx.cs" Inherits="paginas_Nomina_Frm_Rpt_Nom_Formato_Pago_Proveedores"   Title="Formato de Pago a Proveedores" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <script type="text/javascript" language="javascript">
        function Limpiar_Ctlr_Campos(){
            return false;
        }  
        function Limpiar_Ctlr_Busqueda_Empleado(){
            document.getElementById("<%=Txt_Busqueda_No_Empleado.ClientID%>").value="";
            document.getElementById("<%=Txt_Busqueda_RFC.ClientID%>").value="";  
            document.getElementById("<%=Cmb_Busqueda_Dependencia.ClientID%>").value="";  
            document.getElementById("<%=Txt_Busqueda_Nombre_Empleado.ClientID%>").value="";                            
            return false;
        } 
    </script> 

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
            <table width="99%"  border="0" cellspacing="0" class="estilo_fuente">
                <tr align="center">
                    <td class="label_titulo" colspan="4">Solicitud de Pago a Proveedores</td>
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
                    <td align="left" style="width:50%;">
                        &nbsp;
                        <asp:ImageButton ID="Btn_Generar_Documento" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_rep_word.png" Width="24px" ToolTip="Generar Documento" AlternateText="Generar Documento" OnClick="Btn_Generar_Documento_Click" />
                        &nbsp;
                    </td>
                    <td align="right" style="width:50%;">
                        &nbsp;
                        <asp:ImageButton ID="Btn_Limpiar_Campos" runat="server" ToolTip="Limpiar" AlternateText="Limpiar" ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png" Width="24px"  OnClientClick="javascript:return Limpiar_Ctlr_Campos();" />
                        &nbsp;
                    </td>                                      
            </table>  
            <br />  
        </div>

            <div id="Div_Cuerpo" style="background-color:#ffffff; width:100%; height:100%;">
                <table width="99%" class="estilo_fuente">  
                    <tr>
                        <td style="text-align:center; width:100%" >
                            <asp:Panel ID="Pnl_Listado_Filtros" runat="server" GroupingText="Campos Necesarios" Font-Bold="true">
                                <table class="estilo_fuente" width="99%">  
                                    <tr>
                                        <td style="text-align:left; width:15%">
                                            <asp:Label ID="Lbl_No_Oficio" runat="server" Text="No. Oficio"></asp:Label>
                                        </td>
                                        <td style="text-align:left; width:35%">
                                            <asp:TextBox ID="Txt_No_Oficio" runat="server" Width="90%" ></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_No_Oficio" runat="server" TargetControlID="Txt_No_Oficio" FilterType="Numbers, Custom, LowercaseLetters, UppercaseLetters" ValidChars="ñÑ/.-_" >
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:left; width:15%">
                                            <asp:Label ID="Lbl_Proveedor" runat="server" Text="Proveedor"></asp:Label>
                                        </td>
                                        <td style="text-align:left;" colspan="2">
                                            <asp:DropDownList ID="Cmb_Proveedor" runat="server" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="Cmb_Proveedor_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="text-align:left; width:35%">
                                            <asp:TextBox ID="Txt_Monto_Pago_Proveedor" runat="server" Width="100%" style="border-color:Blue; color:Blue; font-weight:bolder; text-align:right;"></asp:TextBox>     
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:left; width:15%">
                                            <asp:Label ID="Lbl_Monto_Letra" runat="server" Text="Monto"></asp:Label>
                                        </td>
                                        <td style="text-align:left;" colspan="3">
                                            <asp:TextBox ID="Txt_Monto_Letra" runat="server" Width="100%" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:left; width:15%">
                                            <asp:Label ID="Lbl_Anio" runat="server" Text="Año"></asp:Label>
                                        </td>
                                        <td style="text-align:left; width:35%">
                                            <asp:DropDownList ID="Cmb_Anio" runat="server" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="Cmb_Anio_SelectedIndexChanged"></asp:DropDownList>
                                        </td>
                                        <td style="text-align:left; width:15%">
                                            &nbsp;
                                            <asp:Label ID="Lbl_Periodo" runat="server" Text="Periodo"></asp:Label>
                                        </td>
                                        <td style="text-align:left; width:35%">
                                            <asp:DropDownList ID="Cmb_Periodo" runat="server" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="Cmb_Periodo_SelectedIndexChanged"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:left; width:15%">
                                            <asp:HiddenField ID="Hdf_Seleccionado" runat="server" />
                                            <asp:Label ID="Lbl_Tesorero_Municipal" runat="server" Text="Tesorero"></asp:Label>
                                        </td>
                                        <td style="text-align:left;" colspan="3">
                                            <asp:TextBox ID="Txt_Tesorero_Municipal" runat="server" Width="90%" Enabled="false"></asp:TextBox>
                                            <asp:ImageButton ID="Btn_Buscar_Terorero" runat="server" ToolTip="Consultar"  ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png"  onclick="Btn_Buscar_Tesorero_Click"  Width="24px"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:left; width:15%">
                                            <asp:Label ID="Lbl_Director_Contabilidad_Presupuesto" runat="server" Text="Dir. Cont."></asp:Label>
                                        </td>
                                        <td style="text-align:left;" colspan="3">
                                            <asp:TextBox ID="Txt_Director_Contabilidad_Presupuesto" runat="server" Width="90%" Enabled="false"></asp:TextBox>
                                            <asp:ImageButton ID="Btn_Buscar_Director_Contabilidad" runat="server" ToolTip="Consultar"  ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png"  onclick="Btn_Buscar_Director_Contabilidad_Click"  Width="24px"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:left; width:15%">
                                            <asp:Label ID="Lbl_Director_RH" runat="server" Text="Dir. RH"></asp:Label>
                                        </td>
                                        <td style="text-align:left;" colspan="3">
                                            <asp:TextBox ID="Txt_Director_Recursos_Humanos" runat="server" Width="90%" Enabled="false"></asp:TextBox>
                                            <asp:ImageButton ID="Btn_Buscar_Director_RH" runat="server" ToolTip="Consultar"  ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png"  onclick="Btn_Buscar_Director_RH_Click"  Width="24px"/>
                                        </td>
                                    </tr>
                                </table> 
                            </asp:Panel>
                        </td>
                        <td align="right" style="width:80%;">&nbsp;</td> 
                    </tr>
                </table>     
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


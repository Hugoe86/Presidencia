﻿<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" 
    AutoEventWireup="true" CodeFile="Frm_Cat_Nom_Act_Datos_Isseg.aspx.cs" 
    Inherits="paginas_Nomina_Frm_Cat_Nom_Act_Datos_Isseg" Title="Actualizar Datos ISSEG" %>
    
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
    <script type="text/javascript" language="javascript">
        function Limpiar_Ctlr(){
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
    
    function Aplica_ISSEG(){
       if( document.getElementById("<%=Chk_Aplica_ISSEG.ClientID %>").checked){
            $('input[id$=Txt_Fecha_Alta_ISSEG]').val('');
            $('input[id$=Txt_Fecha_Alta_ISSEG]').attr('disabled', false);  
       }else{
            $('input[id$=Txt_Fecha_Alta_ISSEG]').val('');
            $('input[id$=Txt_Fecha_Alta_ISSEG]').attr('disabled', true);       
       }
    }    
   </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">

<cc1:ToolkitScriptManager ID="Tsm_Act_Datos_ISSEG" runat="server"  AsyncPostBackTimeout="36000"/>

<asp:UpdatePanel ID="UpPnl_aux_Busqueda" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
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
    
        <table width="98%"  border="0" cellspacing="0">
             <tr>
               <td colspan="2" align="left" class="label_titulo">
                  Catálogo de Actualización de Datos del ISSEG
               </td>            
             </tr>            
             <tr align="center">
                 <td colspan="2">
                     <div align="right" class="barra_busqueda">
                      <table style="width:100%;height:28px;">
                        <tr>
                          <td align="left" style="width:59%;"> 
                            <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button"
                                ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" OnClick="Btn_Modificar_Click" />
                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Salir_Click"/>                                        
                          </td>
                          <td align="right" style="width:41%;">
                            <table style="width:100%;height:28px;">
                                <tr>
                                    <td style="width:100%;vertical-align:top;" align="right">
                                        <asp:ImageButton ID="Btn_Mostrar_Popup_Busqueda" runat="server" ToolTip="Busqueda Avanzada"
                                            ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="24px" Width="24px"
                                            OnClientClick="javascript:$find('Busqueda_Empleados').show();return false;" CausesValidation="false" />
                                    </td>
                                </tr>
                            </table>
                           </td>
                         </tr>
                      </table>
                    </div>
                 </td>
             </tr>
        </table>  
        
        <table width="98%">
            <tr>
                <td style="width:80%; cursor:default; text-align:left;" colspan="4">
                    <hr />
                </td>    
            </tr> 
            <tr>
                <td style="width:20%; cursor:default; text-align:left;">
                    No Empleado
                </td>
                <td style="width:30%; cursor:default; text-align:left;">
                    <asp:TextBox ID="Txt_No_Empleado" runat="server" Width="98%" MaxLength="10" Enabled="false"/>
                    <cc1:TextBoxWatermarkExtender ID="TBW_Txt_Empleados" runat="server" TargetControlID="Txt_No_Empleado"
                        WatermarkCssClass="watermarked2" WatermarkText="No Empleado"/>
                    <cc1:FilteredTextBoxExtender ID="FTxt_No_Empleado" 
                         runat="server" TargetControlID="Txt_No_Empleado" FilterType="Numbers"/>     
                </td>     
                <td style="width:20%; cursor:default; text-align:left;">                
                </td>
                <td style="width:30%; cursor:default; text-align:left;">
                </td>                               
            </tr>
            <tr>
                <td style="width:20%; cursor:default; text-align:left;">
                    Nombre
                </td>
                <td style="width:80%; cursor:default; text-align:left;" colspan="3">
                    <asp:TextBox ID="Txt_Nombre_Empleado" runat="server" Width="99.5%" Enabled="false"/>
                </td>                                    
            </tr>   
            <tr>
                <td style="width:80%; cursor:default; text-align:left;" colspan="4">
                    <hr />
                </td>    
            </tr>        
            <tr>    
                <td style="text-align:left;width:20%;">
                    Aplica ISSEG
                </td>
                <td style="text-align:left;width:30%;">  
                    <asp:CheckBox ID="Chk_Aplica_ISSEG" runat="server"  onclick="javascript:Aplica_ISSEG();" Checked="false"/>
                </td>             
                <td style="text-align:left;width:20%;">
                   &nbsp;&nbsp;Fecha Alta ISSEG
                </td>
                <td style="text-align:left;width:30%;">
                    <asp:TextBox ID="Txt_Fecha_Alta_ISSEG" runat="server"  Width="85%"
                        onblur="$('input[id$=Txt_Fecha_Alta_ISSEG]').filter(function(){if(!this.value.match(/^(0[1-9]|[12][0-9]|3[01])(0[1-9]|1[012])(19|20)\d\d$/))$(this).val('');});"/>
                    <cc1:FilteredTextBoxExtender ID="Fte_Fecha_Alta_ISSEG" 
                        runat="server" TargetControlID="Txt_Fecha_Alta_ISSEG" FilterType="Numbers" />
                    <cc1:CalendarExtender ID="Ce_Fecha_Alta_ISSEG" runat="server" 
                        TargetControlID="Txt_Fecha_Alta_ISSEG" PopupButtonID="Btn_Fecha_Alta_ISSEG" Format="ddMMyyyy"/>
                    <asp:ImageButton ID="Btn_Fecha_Alta_ISSEG" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                        ToolTip="Seleccione la Fecha de Alta de ISSEG"/>   
                </td>
            </tr>
            <tr>
                <td style="width:80%; cursor:default; text-align:left;" colspan="4">
                    <hr />
                </td>    
            </tr>  
        </table>  
    

        <asp:Button ID="Btn_Comodin_MPE_Empleados" runat="server" Text="" style="display:none;"/>
        <cc1:ModalPopupExtender ID="MPE_Empleados" runat="server"  BehaviorID="Busqueda_Empleados"
        TargetControlID="Btn_Comodin_MPE_Empleados" PopupControlID="Pnl_Busqueda_Contenedor" 
        CancelControlID="Btn_Cerrar_Ventana" PopupDragHandleControlID="Pnl_Busqueda_Empleado"
        DropShadow="True" BackgroundCssClass="progressBackgroundFilter"/>  
        
    </ContentTemplate>
</asp:UpdatePanel> 
    
<asp:Panel ID="Pnl_Busqueda_Contenedor" runat="server" CssClass="drag"  HorizontalAlign="Center" Width="850px" 
    style="display:none;border-style:outset;border-color:Silver;background-image:url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG);background-repeat:repeat-y;">                         
    <asp:Panel ID="Pnl_Busqueda_Empleado" runat="server" 
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
                          <div id="Div_Resultados_Busqueda" runat="server" style="border-style:outset; width:99%; height: 250px; overflow:auto;">
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


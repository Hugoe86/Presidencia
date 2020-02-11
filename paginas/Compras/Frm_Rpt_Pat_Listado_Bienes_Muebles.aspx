<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Rpt_Pat_Listado_Bienes_Muebles.aspx.cs" Inherits="paginas_Compras_Frm_Rpt_Pat_Listado_Bienes_Muebles" Title="Listado de Bienes Muebles" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
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
            document.getElementById("<%=Txt_Inventario_Anterior.ClientID%>").value="";
            document.getElementById("<%=Txt_Inventario_SIAS.ClientID%>").value="";  
            document.getElementById("<%=Txt_Nombre_Producto.ClientID%>").value=""; 
            document.getElementById("<%=Cmb_Procedencia.ClientID%>").value=""; 
            document.getElementById("<%=Cmb_Dependencia.ClientID%>").value="";  
            document.getElementById("<%=Txt_Modelo.ClientID%>").value="";
            document.getElementById("<%=Cmb_Marca.ClientID%>").value="";  
            document.getElementById("<%=Cmb_Material.ClientID%>").value="";  
            document.getElementById("<%=Cmb_Color.ClientID%>").value="";  
            document.getElementById("<%=Cmb_Zona.ClientID%>").value="";
            document.getElementById("<%=Txt_Fecha_Adquisicion_Inicial.ClientID%>").value="";  
            document.getElementById("<%=Txt_Fecha_Adquisicion_Final.ClientID%>").value="";  
            document.getElementById("<%=Txt_Fecha_Modifico_Inicio.ClientID%>").value="";  
            document.getElementById("<%=Txt_Fecha_Modifico_Fin.ClientID%>").value="";  
            document.getElementById("<%=Txt_Factura.ClientID%>").value="";  
            document.getElementById("<%=Txt_Numero_Serie.ClientID%>").value="";  
            document.getElementById("<%=Cmb_Estatus.ClientID%>").value="";  
            document.getElementById("<%=Cmb_Estado.ClientID%>").value=""; 
            document.getElementById("<%=Cmb_Clase_Activo.ClientID%>").value=""; 
            document.getElementById("<%=Cmb_Tipo_Activo.ClientID%>").value=""; 
            document.getElementById("<%=Cmb_Estatus_Resguardo.ClientID%>").value=""; 
            document.getElementById("<%=Cmb_Operacion.ClientID%>").value=""; 
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
                        <td class="label_titulo" colspan="4">Listado de Bienes Muebles</td>
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
                            <asp:ImageButton ID="Btn_Generar_Reporte_PDF" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png" Width="24px" ToolTip="Listado para Imprimir" AlternateText="Listado para Imprimir" OnClick="Btn_Generar_Reporte_PDF_Click" />
                            <asp:ImageButton ID="Btn_Generar_Reporte_Excel" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png" Width="24px" ToolTip="Listado a Excel" AlternateText="Listado a Excel"  OnClick="Btn_Generar_Reporte_Excel_Click"/>
                            &nbsp;
                        </td>
                        <td align="right" style="width:50%;">
                            <asp:ImageButton ID="Btn_Limpiar_Campos" runat="server" ToolTip="Limpiar Campos" AlternateText="Limpiar Resguardante" ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png" Width="24px"  OnClientClick="javascript:return Limpiar_Ctlr_Campos();" />
                            &nbsp;
                        </td>                                      
                </table>  
                <br />  
                <table width="100%" class="estilo_fuente">  
                    <tr>
                        <td style="width:18%; text-align:left; ">
                            <asp:Label ID="Lbl_Inventario_Anterior" runat="server" Text="No. Inventario"></asp:Label>
                        </td>
                        <td style="width:32%; text-align:left;">
                            <asp:TextBox ID="Txt_Inventario_Anterior" runat="server" Width="95%"></asp:TextBox>      
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Inventario_Anterior" runat="server" TargetControlID="Txt_Inventario_Anterior" InvalidChars="<,>,&,',!," FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"  ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ /*-+$%&" Enabled="True">        
                            </cc1:FilteredTextBoxExtender>                                                
                        </td>
                        <td style="width:18%; text-align:left;">
                            <asp:Label ID="Lbl_Inventario_SIAS" runat="server" Text="No. Inventario [SIAS]"></asp:Label>
                        </td>
                        <td style="width:32%; text-align:left;">
                            <asp:TextBox ID="Txt_Inventario_SIAS" runat="server" Width="95%"></asp:TextBox>      
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Inventario_SIAS" runat="server" TargetControlID="Txt_Inventario_SIAS" FilterType="Numbers" Enabled="True">        
                            </cc1:FilteredTextBoxExtender>                               
                        </td>
                    </tr>                                               
                    <tr>
                        <td style="width:18%; text-align:left; ">
                            <asp:Label ID="Lbl_Nombre_Producto" runat="server" Text="Nombre Producto"></asp:Label>
                        </td>
                        <td colspan="3" style="text-align:left;">
                            <asp:TextBox ID="Txt_Nombre_Producto" runat="server" Width="98%" ></asp:TextBox>   
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Nombre_Producto" runat="server" TargetControlID="Txt_Nombre_Producto" InvalidChars="<,>,&,',!," FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True">        
                            </cc1:FilteredTextBoxExtender>                                                                                 
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
                            <asp:Label ID="Lbl_Dependencia" runat="server" Text="Unidad Responsable"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="Cmb_Dependencia" runat="server" Width="100%">
                                <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>   
                            </asp:DropDownList>                                   
                        </td>
                    </tr>   
                    <tr>
                        <td style="width:18%; text-align:left; ">
                            <asp:Label ID="Lbl_Zona" runat="server" Text="Zona"></asp:Label>
                        </td>
                        <td colspan="3" style="text-align:left;">
                            <asp:DropDownList ID="Cmb_Zona" runat="server" Width="100%">
                                <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>
                            </asp:DropDownList>                                                 
                        </td> 
                    </tr>                         
                    <tr>
                        <td style="width:18%; text-align:left; ">
                            <asp:Label ID="Lbl_Modelo" runat="server" Text="Modelo"></asp:Label>
                        </td>
                        <td colspan="3" style="text-align:left;">
                            <asp:TextBox ID="Txt_Modelo" runat="server" Width="98%"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Modelo" runat="server" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" TargetControlID="Txt_Modelo" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-_%/$# " Enabled="True">
                            </cc1:FilteredTextBoxExtender>                               
                        </td>
                    </tr>                                                 
                    <tr>
                        <td style="width:18%; text-align:left; ">
                            <asp:Label ID="Lbl_Marca" runat="server" Text="Marca"></asp:Label>
                        </td>
                        <td style="width:32%; text-align:left;">
                            <asp:DropDownList ID="Cmb_Marca" runat="server" Width="98%">
                                <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>            
                            </asp:DropDownList>                             
                        </td>
                        <td style="width:18%; text-align:left; ">
                            <asp:Label ID="Lbl_Material" runat="server" Text="Material"></asp:Label>
                        </td>
                        <td style="width:32%; text-align:left;">
                            <asp:DropDownList ID="Cmb_Material" runat="server" Width="98%">
                                <asp:ListItem Text="&lt; TODOS &gt;" Value="TODOS"></asp:ListItem>            
                            </asp:DropDownList>                             
                        </td>
                    </tr>            
                    <tr>
                        <td style="width:18%; text-align:left; ">
                            <asp:Label ID="Lbl_Color" runat="server" Text="Color"></asp:Label>
                        </td>
                        <td style="width:32%; text-align:left;">
                            <asp:DropDownList ID="Cmb_Color" runat="server" Width="98%">
                                <asp:ListItem Text="&lt; TODOS &gt;" Value="TODOS"></asp:ListItem>
                            </asp:DropDownList>                                                 
                        </td>
                        <td style="width:18%; text-align:left; ">
                            <asp:Label ID="Lbl_Procedencia" runat="server" Text="Procedencia"></asp:Label>
                        </td>
                        <td style="width:32%; text-align:left;">
                            <asp:DropDownList ID="Cmb_Procedencia" runat="server" Width="98%">
                                <asp:ListItem Text="&lt; TODOS &gt;" Value="TODOS"></asp:ListItem>
                            </asp:DropDownList>                                                 
                        </td>
                    </tr>
                    <tr>
                        <td style="width:18%; text-align:left; ">
                            <asp:Label ID="Lbl_Factura" runat="server" Text="No. Factura"></asp:Label>
                        </td>
                        <td style="width:32%; text-align:left;">
                            <asp:TextBox ID="Txt_Factura" runat="server" Width="95%"></asp:TextBox>    
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Factura" runat="server" TargetControlID="Txt_Factura" InvalidChars="<,>,&,',!," FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True">        
                            </cc1:FilteredTextBoxExtender>                                    
                        </td>
                        <td style="width:18%; text-align:left;">
                            <asp:Label ID="Lbl_Numero_Serie" runat="server" Text="No. Serie"></asp:Label>
                        </td>
                        <td style="width:32%; text-align:left;">
                            <asp:TextBox ID="Txt_Numero_Serie" runat="server" Width="95%"></asp:TextBox>      
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Numero_Serie" runat="server" TargetControlID="Txt_Numero_Serie" InvalidChars="<,>,&,',!," FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"  ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ /*-+$%&" Enabled="True">        
                            </cc1:FilteredTextBoxExtender>                               
                        </td>
                    </tr>    
                    <tr>
                        <td style="width:18%; text-align:left;">
                            <asp:Label ID="Lbl_Fecha_Adquisicion_Inicial" runat="server" Text="F. Adquisición [Inicio]"></asp:Label>
                        </td>
                        <td style="width:32%; text-align:left;">
                            <asp:TextBox ID="Txt_Fecha_Adquisicion_Inicial" runat="server" Width="80%" MaxLength="20" Enabled="false" ></asp:TextBox>
                            <asp:ImageButton ID="Btn_Fecha_Adquisicion_Inicial" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                            <cc1:CalendarExtender ID="CE_Txt_Fecha_Adquisicion_Inicial" runat="server" TargetControlID="Txt_Fecha_Adquisicion_Inicial" PopupButtonID="Btn_Fecha_Adquisicion_Inicial" Format="dd/MMM/yyyy">
                            </cc1:CalendarExtender>
                        </td>
                        <td style="width:18%; text-align:left;">
                            <asp:Label ID="Lbl_Fecha_Adquisicion_Final" runat="server" Text="F. Adquisición [Fin]"></asp:Label>
                        </td>
                        <td style="width:32%; text-align:left;">
                            <asp:TextBox ID="Txt_Fecha_Adquisicion_Final" runat="server" Width="80%" MaxLength="20" Enabled="false" ></asp:TextBox>
                            <asp:ImageButton ID="Btn_Fecha_Adquisicion_Final" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                            <cc1:CalendarExtender ID="CE_Txt_Fecha_Adquisicion_Final" runat="server" TargetControlID="Txt_Fecha_Adquisicion_Final" PopupButtonID="Btn_Fecha_Adquisicion_Final" Format="dd/MMM/yyyy">
                            </cc1:CalendarExtender>
                        </td>
                    </tr>         
                    <tr>
                        <td style="width:18%; text-align:left;">
                            <asp:Label ID="Lbl_Fecha_Modifico_Inicio" runat="server" Text="F. Modificación [Inicio]"></asp:Label>
                        </td>
                        <td style="width:32%; text-align:left;">
                            <asp:TextBox ID="Txt_Fecha_Modifico_Inicio" runat="server" Width="80%" MaxLength="20" Enabled="false" ></asp:TextBox>
                            <asp:ImageButton ID="Btn_Fecha_Modifico_Inicio" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                            <cc1:CalendarExtender ID="CE_Txt_Fecha_Modifico_Inicio" runat="server" TargetControlID="Txt_Fecha_Modifico_Inicio" PopupButtonID="Btn_Fecha_Modifico_Inicio" Format="dd/MMM/yyyy">
                            </cc1:CalendarExtender>
                        </td>
                        <td style="width:18%; text-align:left;">
                            <asp:Label ID="Lbl_Fecha_Modifico_Fin" runat="server" Text="F. Modificación [Fin]"></asp:Label>
                        </td>
                        <td style="width:32%; text-align:left;">
                            <asp:TextBox ID="Txt_Fecha_Modifico_Fin" runat="server" Width="80%" MaxLength="20" Enabled="false" ></asp:TextBox>
                            <asp:ImageButton ID="Btn_Fecha_Modifico_Fin" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                            <cc1:CalendarExtender ID="CE_Txt_Fecha_Modifico_Fin" runat="server" TargetControlID="Txt_Fecha_Modifico_Fin" PopupButtonID="Btn_Fecha_Modifico_Fin" Format="dd/MMM/yyyy">
                            </cc1:CalendarExtender>
                        </td>
                    </tr>               
                    <tr>
                        <td style="width:18%; text-align:left; ">
                            <asp:Label ID="Lbl_Estatus" runat="server" Text="Estatus Bien"></asp:Label>
                        </td>
                        <td style="width:32%; text-align:left;">
                            <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="98%">
                                <asp:ListItem Text="&lt; TODOS &gt;" Value=""></asp:ListItem>
                                <asp:ListItem Value="VIGENTE">VIGENTE</asp:ListItem>
                                <asp:ListItem Value="TEMPORAL">BAJA (TEMPORAL)</asp:ListItem>
                                <asp:ListItem Value="DEFINITIVA">BAJA (DEFINITIVA)</asp:ListItem>
                            </asp:DropDownList>                                                 
                        </td>
                        <td style="width:18%; text-align:left; ">
                            <asp:Label ID="Lbl_Estado" runat="server" Text="Estado"></asp:Label>
                        </td>
                        <td style="width:32%; text-align:left;">
                            <asp:DropDownList ID="Cmb_Estado" runat="server" Width="98%">
                                <asp:ListItem Text="&lt; TODOS &gt;" Value=""></asp:ListItem>
                                <asp:ListItem Value="BUENO">BUENO</asp:ListItem>
                                <asp:ListItem Value="REGULAR">REGULAR</asp:ListItem>
                                <asp:ListItem Value="MALO">MALO</asp:ListItem>
                            </asp:DropDownList>                                                 
                        </td>
                    </tr>  
                    <tr>
                        <td style="width:18%; text-align:left; ">
                            <asp:Label ID="Lbl_Estatus_Resguardo" runat="server" Text="Estatus Resguardante"></asp:Label>
                        </td>
                        <td style="width:32%; text-align:left;">
                            <asp:DropDownList ID="Cmb_Estatus_Resguardo" runat="server" Width="98%">
                                <asp:ListItem Text="&lt; TODOS &gt;" Value=""></asp:ListItem>
                                <asp:ListItem Value="VIGENTE">VIGENTE</asp:ListItem>
                                <asp:ListItem Value="BAJA">BAJA</asp:ListItem>
                            </asp:DropDownList>                                                 
                        </td>
                        <td style="text-align:left; width:18%;">
                            <asp:Label ID="Lbl_Operación" runat="server" Text="Operación" ></asp:Label>
                        </td>
                        <td style="width:32%">
                            <asp:DropDownList ID="Cmb_Operacion" runat="server" Width="98%" >
                                <asp:ListItem Text="&lt; TODOS &gt;" Value=""></asp:ListItem>
                                <asp:ListItem Value="RECIBO">RECIBO</asp:ListItem>
                                <asp:ListItem Value="RESGUARDO">RESGUARDO</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>                                              
                    <tr>
                        <td style="width:18%; text-align:left; ">
                            <asp:HiddenField ID="Hdf_Resguardante_ID" runat="server" />
                            <asp:Label ID="Lbl_Resguardante" runat="server" Text="Resguardante"></asp:Label>
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
                                                <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" SortExpression="ESTATUS" >
                                                    <ItemStyle Width="70px" Font-Size="X-Small" HorizontalAlign="Center" />
                                                    <HeaderStyle Width="70px" Font-Size="X-Small" HorizontalAlign="Center" />
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


<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Tiempo_Extra_Empleados.aspx.cs" Inherits="paginas_Nomina_Frm_Ope_Tiempo_Extra_Empleados" Title="Tiempo Extra" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">

 <script src="../../javascript/Js_Ope_Nom_Tiempo_Extra.js" type="text/javascript"></script>

 <script type="text/javascript" language="javascript">
     function calendarShown(sender, args) {
         sender._popupBehavior._element.style.zIndex = 10000005;
     }
     function Limpiar_Ctlr() {
         document.getElementById("<%=Txt_Busqueda_No_Tiempo_Extra.ClientID %>").value = "";
         document.getElementById("<%=Cmb_Busqueda_Estatus.ClientID %>").value = "";
         document.getElementById("<%=Txt_Busqueda_Fecha_Inicio.ClientID %>").value = "";
         document.getElementById("<%=Txt_Busqueda_Fecha_Fin.ClientID %>").value = "";
         document.getElementById("<%=Txt_Busqueda_No_Empleado.ClientID %>").value = "";
         return false;
     }

     function pageLoad(sender, args) {
         $('textarea[id*=Txt_Comentarios]').keyup(function() { var Caracteres = $(this).val().length; if (Caracteres > 250) { this.value = this.value.substring(0, 250); Caracteres = $(this).val().length; $(this).css("background-color", "Yellow"); $(this).css("color", "Red"); } else { $(this).css("background-color", "White"); $(this).css("color", "Black"); } $('#Contador_Caracteres_Comentarios').text('Carácteres Ingresados [ ' + Caracteres + ' ]/[ 250 ]'); });

         $('input[id$=Txt_Busqueda_No_Empleado]').live("blur", function() {
             if (isNumber($(this).val())) {
                 var Ceros = "";
                 if ($(this).val() != undefined) {
                     if ($(this).val() != '') {
                         for (i = 0; i < (6 - $(this).val().length); i++) {
                             Ceros += '0';
                         }
                         $(this).val(Ceros + $(this).val());
                         Ceros = "";
                     } else $(this).val('');
                 }
             }
         });

         $('input[id$=Txt_Busqueda_No_Tiempo_Extra]').live("blur", function() {
             if (isNumber($(this).val())) {
                 var Ceros = "";
                 if ($(this).val() != undefined) {
                     if ($(this).val() != '') {
                         for (i = 0; i < (10 - $(this).val().length); i++) {
                             Ceros += '0';
                         }
                         $(this).val(Ceros + $(this).val());
                         Ceros = "";
                     } else $(this).val('');
                 }
             }
         });

         $('input[id$=Btn_Busqueda_Tiempo_Horas_Extra]').hover(
            function(e) {
                e.preventDefault();
                $(this).css("background-color", "#2F4E7D");
                $(this).css("color", "#FFFFFF");
            },
            function(e) {
                e.preventDefault();
                $(this).css("background-color", "#f5f5f5");
                $(this).css("color", "#565656");
            }
        );
        }
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


     function isNumber(n) { return !isNaN(parseFloat(n)) && isFinite(n); }
 </script>
 
 
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">    
    <cc1:ToolkitScriptManager ID="Tsm_Tiempo_Extra_Empleados" runat="server"  AsyncPostBackTimeout="36000"  EnableScriptGlobalization = "true" EnableScriptLocalization = "True"/>    
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>  
               
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            
            <asp:Button ID="Button1" runat="server" style="background-color:Transparent;border-style:none;" OnClientClick="javascript:return false;"/>
            
             <div id="Div_Tiempo_Extra_Empleados" style="background-color:#ffffff; width:100%; height:100%;">
            
                    <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                        <tr align="center">
                            <td class="label_titulo">Tiempo Extra</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false"  />&nbsp;
                                <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"/>
                            </td>
                        </tr>
                    </table>                                                             
                    
                    <table width="98%"  border="0" cellspacing="0">
                             <tr align="center">
                                 <td colspan="2">                
                                     <div align="right" class="barra_busqueda">                        
                                          <table style="width:100%;height:28px;">
                                            <tr>
                                              <td align="left" style="width:59%;">                                                  
                                                   <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button"  
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" onclick="Btn_Nuevo_Click" />
                                                    <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button" 
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" onclick="Btn_Modificar_Click" />
                                                    <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar" CssClass="Img_Button" 
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png" onclick="Btn_Eliminar_Click"
                                                        OnClientClick="return confirm('¿Está seguro de eliminar el Tiempo Extra seleccionado?');"/>
                                                    <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" 
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" onclick="Btn_Salir_Click"/>
                                              </td>
                                              <td align="right" style="width:41%;">
                                                <table style="width:100%;height:28px;">
                                                    <tr>
                                                        <td style="width:60%;vertical-align:top;">
                                                            B&uacute;squeda
                                                            <asp:ImageButton ID="Btn_Busqueda_Tiempo_Extra_Empleado" runat="server" ToolTip="Avanzada"  
                                                                ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="24px" Width="24px"
                                                                OnClientClick="javascript:$find('Busqueda_Empleados').show();return false;" CausesValidation="false"/>                                        
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

                <br />
                                
                <table width="98%">
                    <tr style="display:none">
                        <td style="text-align:left;width:20%;">
                            No Tiempo Extra
                        </td>
                        <td  style="text-align:left;width:30%;" >
                            <asp:TextBox ID="Txt_No_Tiempo_Extra_Empleado" runat="server" Width="98%" />
                        </td> 
                        <td style="text-align:left;width:20%;">
                        </td>
                        <td  style="text-align:left;width:30%;">                            
                        </td>                                                                       
                    </tr>    
                    <tr id="Tr_Periodos_Fiscales" runat="server">
                        <td style="text-align:left;width:20%;">
                            N&oacute;mina
                        </td>
                        <td  style="text-align:left;width:30%;" >
                            <asp:DropDownList ID="Cmb_Calendario_Nomina" runat="server" Width="100%" AutoPostBack="true" 
                                OnSelectedIndexChanged="Cmb_Calendario_Nomina_SelectedIndexChanged"/>
                        </td> 
                        <td style="text-align:left;width:20%;">
                           &nbsp;&nbsp;Periodo
                        </td>
                        <td  style="text-align:left;width:30%;">   
                            <asp:DropDownList ID="Cmb_Periodos_Catorcenales_Nomina" runat="server" Width="100%"/>
                        </td>                                                                       
                    </tr>                                    
                    <tr>
                        <td style="text-align:left;width:20%;">                                                       
                            *Estatus
                        </td>
                        <td  style="text-align:left;width:30%;">                            
                            <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="100%" >
                                <asp:ListItem>&lt; Seleccione &gt;</asp:ListItem>
                                <asp:ListItem>Pendiente</asp:ListItem>   
                                <asp:ListItem>Aceptado</asp:ListItem>                                
                                <asp:ListItem>Rechazado</asp:ListItem>                                                                                               
                            </asp:DropDownList>
                        </td>  
                        <td style="text-align:left;width:20%;">
                            *Fecha
                        </td>
                        <td style="text-align:left;width:30%;">     
                            <asp:TextBox ID="Txt_Fecha" runat="server" Width="85%" MaxLength="1"  Wrap="true"/>
                            <cc1:FilteredTextBoxExtender ID="Txt_Fecha_FilteredTextBoxExtender" 
                                runat="server" TargetControlID="Txt_Fecha" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" ValidChars="/"/>
                            <cc1:CalendarExtender ID="Txt_Fecha_CalendarExtender" runat="server"
                                TargetControlID="Txt_Fecha" PopupButtonID="Btn_Fecha" Format="dd/MMM/yyyy" />
                            <asp:ImageButton ID="Btn_Fecha" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                ToolTip="Seleccione la Fecha"/>                                   
                        </td>                        
                    </tr>     
                    <tr>
                        <td style="text-align:left;width:20%;">                                                       
                            *Pago Dia Doble
                        </td>
                        <td  style="text-align:left;width:30%;">                            
                            <asp:DropDownList ID="Cmb_Pago_Dia_Doble" runat="server" Width="100%"  AutoPostBack="true"
                                OnSelectedIndexChanged="Cmb_Pago_Dia_Doble_SelectedIndexChanged">
                                <asp:ListItem>&lt; Seleccione &gt;</asp:ListItem>
                                <asp:ListItem>SI</asp:ListItem>
                                <asp:ListItem>NO</asp:ListItem>
                            </asp:DropDownList>
                        </td>  
                        <td style="text-align:left;width:20%;">
                            *Horas
                        </td>
                        <td style="text-align:left;width:30%;">     
                            <asp:TextBox ID="Txt_Horas" runat="server" Width="98%" MaxLength="20" />
                            <cc1:FilteredTextBoxExtender ID="FTxt_Horas" 
                                runat="server" TargetControlID="Txt_Horas" FilterType="Numbers"/>  
                            <asp:CustomValidator ID="Cv_Txt_Horas" runat="server"  Display="None"
                                 EnableClientScript="true" ErrorMessage="Las Horas Extra no pueden ser 0 Hrs."
                                 Enabled="true"
                                 ClientValidationFunction="TextBox_Txt_Horas"
                                 HighlightCssClass="highlight" 
                                 ControlToValidate="Txt_Horas"/>
                            <cc1:ValidatorCalloutExtender ID="Vce_Txt_Horas" runat="server" TargetControlID="Cv_Txt_Horas" PopupPosition="TopRight"/>    
                            <script type="text/javascript" >
                                function TextBox_Txt_Horas(sender, args) {     
                                     var Horas = document.getElementById("<%=Txt_Horas.ClientID%>").value;   
                                     if (Horas <= 0){   
                                        document.getElementById("<%=Txt_Horas.ClientID%>").value ="";      
                                        args.IsValid = false;     
                                     }
                                  } 
                            </script>                                                           
                        </td>                        
                    </tr>                       
                    <tr>
                        <td style="text-align:left;width:20%;vertical-align:top;">
                            Comentarios
                        </td>
                        <td  style="text-align:left;width:80%;" colspan="3">
                            <asp:TextBox ID="Txt_Comentarios" runat="server" Width="99.5%" TextMode="MultiLine"
                                Height="60px" MaxLength="1" Wrap="true"/>
                            <cc1:FilteredTextBoxExtender ID="FTxt_Comentarios" runat="server"  TargetControlID="Txt_Comentarios"
                                FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars="áéíóúÁÉÍÓÚ ñÑ"/>    
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Comentarios" runat="server" TargetControlID ="Txt_Comentarios" 
                                WatermarkText="Límite de Caractes 250" WatermarkCssClass="watermarked"/>       
                            <span id="Contador_Caracteres_Comentarios" class="watermarked"></span>                                                  
                        </td>
                    </tr>    
                    <tr>
                        <td style="width:100%;vertical-align:top;" colspan="4" align="right">
                            <asp:HyperLink ID="Btn_Autorizacion_Horas_Extras" runat="server" Text="Autorizacion Horas Extra" Visible="false" ForeColor="#0000FF"/>
                        </td>
                    </tr>                                                                                                                                                                           
                </table>
                
                <br />

                <cc1:TabContainer ID="TPnl_Contenedor" runat="server" ActiveTabIndex="0" Width="98%" CssClass="Tab">
                    <cc1:TabPanel ID="Pnl_Horas_Extra" runat="server" HeaderText="Horas Extra">
                        <HeaderTemplate>
                            Horas Extra
                        </HeaderTemplate>
                        <ContentTemplate>
                                    <div style="overflow:auto;height:400px;width:99%;vertical-align:top;border-style: outset;color:White;" >
                                        <asp:GridView ID="Grid_Horas_Extra" runat="server" CssClass="GridView_1" Width="100%"
                                             AutoGenerateColumns="False"  GridLines="None" AllowPaging="true" PageSize="10"
                                             onpageindexchanging="Grid_Horas_Extra_PageIndexChanging"
                                             OnRowDataBound="Grid_Horas_Extra_RowDataBound"
                                             OnSelectedIndexChanged="Grid_Horas_Extra_SelectedIndexChanged">
                                                <Columns>
                                                    <asp:ButtonField ButtonType="Image" CommandName="Select"  HeaderText="Seleccionar"
                                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                        <ItemStyle Width="5%"  HorizontalAlign="Left"/>
                                                        <HeaderStyle HorizontalAlign="Left" Width="5%"/>
                                                    </asp:ButtonField>                                                
                                                    <asp:BoundField DataField="NO_TIEMPO_EXTRA" HeaderText="No Hora Extra">
                                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="FECHA" HeaderText="Fecha"  DataFormatString="{0:dd/MMM/yyyy}">
                                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                    </asp:BoundField>                                                       
                                                    <asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                    </asp:BoundField>  
                                                    <asp:BoundField DataField="DEPENDENCIA" HeaderText="U. Responsable">
                                                        <HeaderStyle HorizontalAlign="Left" Width="30%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="30%" />
                                                    </asp:BoundField> 
                                                    
                                                          
                                                    <asp:TemplateField HeaderText="Estatus">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="Btn_Reporte_Tiempo_Extra" runat="server" 
                                                                   ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png"  Width="24px"
                                                                   OnClick="Btn_Reporte_Tiempo_Extra_Click" 
                                                                   CausesValidation="false"/>                                                    
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Width="1%" Height="1%"/>
                                                            <ItemStyle HorizontalAlign="Center" Width="1%" Height="1%"/>                                                        
                                                        </asp:TemplateField>                                                         
                                                    
                                                                                                                                                                                                                                                
                                                </Columns>
                                                <SelectedRowStyle CssClass="GridSelected" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <HeaderStyle CssClass="GridHeader" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                            </asp:GridView>   
                                       </div>                        
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="Pnl_Empleados" runat="server" HeaderText="Empleados">
                        <HeaderTemplate>
                            Empleados
                        </HeaderTemplate>
                        <ContentTemplate>
                             <table style="width:100%;">
                                <tr>
                                    <td style="text-align:left;width:20%;"> 
                                        U. Responsable
                                    </td>
                                    <td style="text-align:left;width:80%;" colspan="3">
                                        <asp:DropDownList ID="Cmb_Dependencia" runat="server" Width="100%"  AutoPostBack="true" 
                                            OnSelectedIndexChanged="Cmb_Dependencias_SelectedIndexChanged"/>                                            
                                    </td>
                                    </tr>
                                <tr>
                                    <td style="text-align:left;width:20%;">                          
                                        *Empleado
                                    </td>
                                    <td style="text-align:left;width:30%;">     
                                        <asp:TextBox ID="Txt_Empleados" runat="server" Width="98%" style="font-size:12px" OnTextChanged="Txt_Empleados_TextChanged" AutoPostBack="true"/>
                                        <cc1:TextBoxWatermarkExtender ID="TBW_Txt_Empleados" runat="server" TargetControlID="Txt_Empleados"
                                            WatermarkCssClass="watermarked2" WatermarkText="Nombre ó Empleado ID"/>
                                    </td>
                                    <td style="text-align:left;width:5%";>
                                        <asp:ImageButton ID="Btn_Buscar_Empleados" runat="server" Text="Buscar" 
                                             ImageUrl="~/paginas/imagenes/paginas/busqueda.png" OnClick="Btn_Buscar_Empleados_Click"/>
                                    </td>
                                    <td style="text-align:left;width:45%;">
                                        <asp:DropDownList ID="Cmb_Empleados" runat="server" Width="70%"  style="font-size:11px"/>
                                        <asp:Button ID="Btn_Agregar_Empleado" runat="server" Text="Agregar"  
                                            OnClick="Btn_Agregar_Empleado_Click" Width="27%" Font-Size="11px" Height="22px"/>
                                    </td>
                                </tr>      
                                <tr>
                                    <td style="width:100%;" colspan="4">
                                        <div style="overflow:auto;height:400px;width:99%;vertical-align:top;border-style:outset;color:White;" >
                                            <asp:GridView ID="Grid_Tiempo_Extra_Empleado" runat="server" CssClass="GridView_1"
                                                 AutoGenerateColumns="False"  GridLines="Vertical" EmptyDataText ="No se encontraron registros"
                                                 OnRowDataBound="Grid_Tiempo_Extra_Empleado_RowDataBound">
                                                    <Columns>                                              
                                                        <asp:BoundField DataField="EMPLEADO_ID" HeaderText="Empleado ID">
                                                            <ItemStyle Font-Size="X-Small" HorizontalAlign="Left" Width="0%"/>
                                                            <HeaderStyle Font-Size="X-Small" HorizontalAlign="Center" Width="0%"/>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="NOMBRE" HeaderText="Nombre">
                                                            <ItemStyle Font-Size="X-Small" HorizontalAlign="Left" Width="80%"/>
                                                            <HeaderStyle Font-Size="X-Small" HorizontalAlign="Center" Width="80%"/>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Eliminar">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="Btn_Eliminar_Empleado" runat="server" ImageUrl="~/paginas/imagenes/gridview/grid_garbage.png" 
                                                                   OnClick="Btn_Eliminar_Empleado_Click" OnClientClick="return confirm('¿Está seguro de eliminar el Empelado seleccionado?');"/>                                                    
                                                            </ItemTemplate>
                                                            <ItemStyle Font-Size="X-Small" HorizontalAlign="Center" Width="20%"/>
                                                            <HeaderStyle Font-Size="X-Small" HorizontalAlign="Center" Width="20%"/>                                                  
                                                        </asp:TemplateField>                                                         
                                                    </Columns>
                                                    <SelectedRowStyle CssClass="GridSelected" />
                                                    <PagerStyle CssClass="GridHeader" />
                                                    <HeaderStyle CssClass="GridHeader" />
                                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                                </asp:GridView>    
                                            </div>                                
                                        </td>
                                    </tr>                                              
                                </table>                             
                        </ContentTemplate>
                    </cc1:TabPanel>                        
                </cc1:TabContainer>                                                              
            </div>
            
                                                                     
        </ContentTemplate>        
         <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Btn_Busqueda_Tiempo_Horas_Extra" EventName="Click"/>
        </Triggers>
        
    </asp:UpdatePanel>     
    
    <asp:UpdatePanel ID="Pnl_Modal" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
            <cc1:ModalPopupExtender ID="MPE_Msj" runat="server" BackgroundCssClass="popUpStyle"  BehaviorID="Busqueda_Empleados"
                PopupControlID="Pnl_Mensaje" TargetControlID="Btn_Axuliar" PopupDragHandleControlID="Pnl_Interno" 
                CancelControlID="Btn_Comodin" DropShadow="True" DynamicServicePath="" Enabled="True" />  
            <asp:Button Style="background-color: transparent; border-style:none;" ID="Btn_Comodin" runat="server" Text="" />  
            <asp:Button Style="background-color: transparent; border-style:none;" ID="Btn_Axuliar" runat="server" Text="" />   
    </ContentTemplate>
         <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Btn_Busqueda_Tiempo_Horas_Extra" EventName="Click"/>
        </Triggers>    
    </asp:UpdatePanel>

    
<asp:Panel ID="Pnl_Mensaje" runat="server" CssClass="drag" HorizontalAlign="Center" Width="650px" 
    style="display:none;border-style:outset;border-color:Silver;background-image:url(../imagenes/paginas/fondo1.jpg);background-repeat:repeat-y;">  
                           
    <asp:Panel ID="Pnl_Interno" runat="server" 
        style="cursor: move;background-color:Silver;color:White;font-size:12;font-weight:bold;border-style:outset;">
        <table width="99%">
            <tr>
                <td style="color:Black;font-size:12;font-weight:bold;text-align:left;">
                   <asp:Image ID="Image1" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                     &nbsp;&nbsp;Informacion: B&uacute;squeda de Horas Extras en el Sistema SIAG
                </td>
                <td align="right" style="width:10%;">
                   <asp:ImageButton ID="Btn_Cerrar_Ventana_Emergente" CausesValidation="false" runat="server" style="cursor:pointer;" ToolTip="Cerrar Ventana"
                        ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png" OnClientClick="javascript:$find('Busqueda_Empleados').hide(); return false;"/>  
                </td>
            </tr>
        </table>            
    </asp:Panel> 
    
                 
        <asp:UpdatePanel ID="a" runat="server" UpdateMode="Conditional" >
            <ContentTemplate>                                                                         
                <div style="cursor:default;width:100%">
                    <table width="97%" style="background-color:Transparent;">
                       <tr>
                            <td style="width:100%" colspan="4" align="right">
                                <asp:ImageButton ID="Btn_Limpiar_Ctlr_Busqueda" runat="server" OnClientClick="javascript:return Limpiar_Ctlr();"
                                    ImageUrl="~/paginas/imagenes/paginas/sias_clear.png" ToolTip="Limpiar Controles de Busqueda" style="cursor:hand;"/>                         
                            </td>
                        </tr>            
                       <tr>
                            <td style="width:100%" colspan="4">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td  style="width:20%;text-align:left;font-size:11px;">
                               No Empleado 
                            </td>              
                            <td style="width:30%;text-align:left;font-size:11px;">
                               <asp:TextBox ID="Txt_Busqueda_No_Empleado" runat="server" Width="98%" MaxLength="6"/>
                               <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_No_Empleado" runat="server" FilterType="Numbers"
                                    TargetControlID="Txt_Busqueda_No_Empleado"/>
                                <cc1:TextBoxWatermarkExtender ID="Twm_Txt_Busqueda_No_Empleado" runat="server" 
                                    TargetControlID ="Txt_Busqueda_No_Empleado" WatermarkText="No Empleado" 
                                    WatermarkCssClass="watermarked"/>                                                                                                                                          
                            </td> 
                            <td style="width:20%;text-align:left;font-size:11px;">                                            
                            </td>              
                            <td style="width:30%;text-align:left;font-size:11px;">                                         
                            </td>                                         
                        </tr>                 
                        <tr>
                            <td style="width:20%;text-align:left;font-size:11px;">
                                No Tiempo Extra
                            </td>
                            <td style="width:30%;text-align:left;font-size:11px;">
                                <asp:TextBox ID="Txt_Busqueda_No_Tiempo_Extra" runat="server" Width="98%" MaxLength="10"/>
                                <cc1:FilteredTextBoxExtender ID="FTxt_Busqueda_No_Tiempo_Extra" 
                                        runat="server" TargetControlID="Txt_Busqueda_No_Tiempo_Extra" FilterType="Numbers"/>  
                                <cc1:TextBoxWatermarkExtender ID="Twm_Busqueda_No_Tiempo_Extra" runat="server" 
                                    TargetControlID ="Txt_Busqueda_No_Tiempo_Extra" WatermarkText="No Tiempo Extra" 
                                    WatermarkCssClass="watermarked"/>                                                                
                            </td>
                            <td style="width:20%;text-align:left;font-size:11px;">
                                Estatus
                            </td>                    
                            <td style="width:30%;text-align:left;font-size:11px;">
                                <asp:DropDownList ID="Cmb_Busqueda_Estatus" runat="server" Width="100%">
                                        <asp:ListItem>&lt; Seleccione &gt;</asp:ListItem>
                                        <asp:ListItem>Pendiente</asp:ListItem>
                                        <asp:ListItem>Aceptado</asp:ListItem>                                
                                        <asp:ListItem>Rechazado</asp:ListItem>                                
                                </asp:DropDownList>                    
                            </td>                                                            
                        </tr> 
                        
                        <tr>
                            <td  style="width:20%; text-align:left; cursor:default;">
                                N&oacute;mina
                            </td>
                            
                            <td style="width:30%; text-align:left; cursor:default;">
                                <asp:DropDownList ID="Cmb_Busqueda_Calendario_Nomina" runat="server" Width="95%" AutoPostBack="true" 
                                    TabIndex="3" onselectedindexchanged="Cmb_Busqueda_Calendario_Nomina_SelectedIndexChanged"/>
                            </td> 
                            
                            <td style="width:20%; text-align:left; cursor:default;">
                                Periodo
                            </td>
                            
                            <td style="width:30%; text-align:left; cursor:default;">
                                <asp:DropDownList ID="Cmb_Busqueda_Periodos_Catorcenales_Nomina" runat="server" Width="95%" TabIndex="4" 
                                    AutoPostBack="true" 
                                    onselectedindexchanged="Cmb_Busqueda_Periodos_Catorcenales_Nomina_SelectedIndexChanged" />
                            </td> 
                        </tr>
                        
                        <tr>
                            <td style="width:20%;text-align:left;font-size:11px;">
                                U. Responsable
                            </td>
                            <td style="width:80%;text-align:left;font-size:11px;" colspan="3">
                                <asp:DropDownList ID="Cmb_Busqueda_Dependencia" runat="server" Width="100%" Enabled="false"/>
                            </td>                                                            
                        </tr>                      
                        <tr>
                            <td style="width:20%;text-align:left;font-size:11px;">
                                Fecha Inicio
                            </td>
                            <td style="width:30%;text-align:left;font-size:11px;">
                                    <asp:TextBox ID="Txt_Busqueda_Fecha_Inicio" runat="server" Width="85%" MaxLength="1"/>
                                    <cc1:FilteredTextBoxExtender ID="FTxt_Busqueda_Fecha_Inicio" 
                                        runat="server" TargetControlID="Txt_Busqueda_Fecha_Inicio" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                        ValidChars="/"/>
                                    <cc1:CalendarExtender ID="CE_Txt_Busqueda_Fecha_Inicio" runat="server" 
                                        TargetControlID="Txt_Busqueda_Fecha_Inicio" PopupButtonID="Btn_Busqueda_Fecha_Inicio" Format="dd/MMM/yyyy" 
                                        OnClientShown="calendarShown"/>
                                    <asp:ImageButton ID="Btn_Busqueda_Fecha_Inicio" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                        ToolTip="Seleccione la Fecha"/>                            
                            </td>
                            <td style="width:20%;text-align:left;font-size:11px;">
                                Fecha Fin
                            </td>
                            <td style="width:30%;text-align:left;font-size:11px;">
                                    <asp:TextBox ID="Txt_Busqueda_Fecha_Fin" runat="server" Width="85%" MaxLength="1"/>
                                    <cc1:FilteredTextBoxExtender ID="FTxt_Busqueda_Fecha_Fin" 
                                        runat="server" TargetControlID="Txt_Busqueda_Fecha_Fin" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                        ValidChars="/"/>
                                    <cc1:CalendarExtender ID="CE_Txt_Busqueda_Fecha_Fin" runat="server" 
                                        TargetControlID="Txt_Busqueda_Fecha_Fin" PopupButtonID="Btn_Busqueda_Fecha_Fin" Format="dd/MMM/yyyy" 
                                        OnClientShown="calendarShown"/>
                                    <asp:ImageButton ID="Btn_Busqueda_Fecha_Fin" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                        ToolTip="Seleccione la Fecha"/>                     
                            </td>                                                            
                        </tr>  
                       <tr>
                            <td style="width:100%" colspan="4">
                                <hr />
                            </td>
                        </tr>                    
                       <tr>
                            <td style="width:100%" colspan="4" align="center">
                                <asp:Button ID="Btn_Busqueda_Tiempo_Horas_Extra" runat="server" Text="Búsqueda Horas Extras" OnClick="Btn_Busqueda_Tiempo_Horas_Extra_Click" CssClass="button_autorizar"
                                    style="background-image:url(../imagenes/paginas/busqueda.png); background-repeat: no-repeat; background-position:right; cursor:hand;"  ToolTip="Ejecuatar la búsqueda de Horas Extras en el sistema"/>   
                            </td>
                        </tr>                  
                       <tr>
                            <td style="width:100%" colspan="4">
                                <hr />
                            </td>
                        </tr>                                            
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>                
    </asp:Panel>       
</asp:Content>


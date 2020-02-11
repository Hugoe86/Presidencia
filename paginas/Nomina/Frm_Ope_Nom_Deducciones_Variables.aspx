<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Nom_Deducciones_Variables.aspx.cs" Inherits="paginas_Nomina_Frm_Ope_Nom_Deducciones_Variables" Title="Deducciones Variables" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">

<script src="../../javascript/Js_Deducciones_Variables.js" type="text/javascript"></script>
 
 <script type="text/javascript" language="javascript">
     function Limpiar_Ctlr() {
            document.getElementById("<%=Txt_Busqueda_No_Empleado.ClientID %>").value = "";
            document.getElementById("<%=Txt_Busqueda_Nombre_Empleado.ClientID %>").value = "";
            document.getElementById("<%=Txt_Busqueda_No_Deduccion.ClientID %>").value="";
            document.getElementById("<%=Cmb_Busqueda_Estatus.ClientID %>").value="";
            return false;
        }
        function pageLoad(sender, args) {

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
        }
        function isNumber(n) { return !isNaN(parseFloat(n)) && isFinite(n); }    
        
        
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
 </script>
 
    <script src="../../easyui/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script src="../../easyui/jquery.formatCurrency-1.4.0.min.js" type="text/javascript"></script>
    <script src="../../easyui/jquery.formatCurrency.all.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <cc1:ToolkitScriptManager ID="Tsm_Deducciones_Variables" runat="server"  AsyncPostBackTimeout="36000"  EnableScriptGlobalization = "true" EnableScriptLocalization = "True"/>
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>  
               
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            
            <asp:Button ID="Btn_Comodin_Perder_Foco" runat="server" style="background-color:Transparent;border-style:none;" OnClientClick="javascript:return false;"/>
            
             <div id="Div_Deducciones_Variables" style="background-color:#ffffff; width:100%; height:100%;">
            
                    <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                        <tr align="center">
                            <td class="label_titulo">Deducciones Variables</td>
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
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" onclick="Btn_Nuevo_Click" CausesValidation="false"/>
                                                    <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button" 
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" onclick="Btn_Modificar_Click" CausesValidation="false"/>
                                                    <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar" CssClass="Img_Button"
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png" onclick="Btn_Eliminar_Click" CausesValidation="false"
                                                        OnClientClick="return confirm('¿Está seguro de eliminar el numero de duduccion variable seleccionado?');"/>
                                                    <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" onclick="Btn_Salir_Click" CausesValidation="false"/>
                                              </td>
                                              <td align="right" style="width:41%;">
                                                <table style="width:100%;height:28px;">
                                                    <tr>
                                                        <td style="width:60%;vertical-align:top;">
                                                            B&uacute;squeda
                                                            <asp:ImageButton ID="Btn_Busqueda_No_Deduccion_Variable" runat="server" ToolTip="Avanzada"  
                                                                ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="24px" Width="24px" CausesValidation="false"/>                                        
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
                            No Deduccion
                        </td>
                        <td  style="text-align:left;width:30%;" >
                            <asp:TextBox ID="Txt_No_Deduccion" runat="server" Width="98%"/>
                        </td> 
                    </tr>
                    <tr>    
                        <td></td>
                        <td></td>
                        <td style="text-align:left;width:20%;">
                            &nbsp;&nbsp;*Estatus
                        </td>
                        <td  style="text-align:left;width:30%;">   
                            <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="100%">
                                <asp:ListItem>&lt; Seleccione &gt;</asp:ListItem>
                                <asp:ListItem>Pendiente</asp:ListItem>
                                <asp:ListItem>Aceptado</asp:ListItem>                                
                                <asp:ListItem>Rechazado</asp:ListItem>                                                                
                            </asp:DropDownList>
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
                           Deducciones
                        </td>
                        <td  style="text-align:left;width:80%;" colspan="3">
                           <asp:DropDownList ID="Cmb_Deducciones" runat="server" Width="100%"/>
                        </td>                                                                       
                    </tr>                                                              
                    <tr>
                        <td style="text-align:left;width:20%;vertical-align:top;">
                            Comentarios
                        </td>
                        <td  style="text-align:left;width:80%;" colspan="3">
                            <asp:TextBox ID="Txt_Comentarios" runat="server" Width="99.5%" MaxLength="100" TextMode="MultiLine" Wrap="true"
                                Height="60px"/>
                            <cc1:FilteredTextBoxExtender ID="FTxt_Comentarios" runat="server"  TargetControlID="Txt_Comentarios"
                                FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars="áéíóúÁÉÍÓÚ ñÑ"/>    
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Comentarios" runat="server" TargetControlID ="Txt_Comentarios" 
                                WatermarkText="Límite de Caractes 250" WatermarkCssClass="watermarked"/>            
                            <span id="Contador_Caracteres_Comentarios" class="watermarked"></span>                                         
                        </td>
                    </tr>    
                    <tr>
                        <td style="width:100%;vertical-align:top;" colspan="4" align="right">
                            <asp:HyperLink ID="Btn_Autorizacion_Deduccion_Variable" runat="server" Text="Autorizacion Deducción Variable" Visible="false" ForeColor="#0000FF"
                                CausesValidation="false"/>
                        </td>
                    </tr>                                                                                                                                                                           
                </table>

                <cc1:TabContainer ID="TPnl_Contenedor" runat="server" ActiveTabIndex="0" Width="98%" CssClass="Tab">
                    <cc1:TabPanel ID="Pnl_No_Deducciones_Variables" runat="server" HeaderText="Deducciones Variables">
                        <HeaderTemplate>
                            Deducciones Variables
                        </HeaderTemplate>
                        <ContentTemplate>
                                        <asp:GridView ID="Grid_Deducciones_Variables" runat="server" CssClass="GridView_1" Width="100%"
                                             AutoGenerateColumns="False"  GridLines="None" AllowPaging="true" PageSize="10"
                                             onpageindexchanging="Grid_Deducciones_Variables_PageIndexChanging"
                                             OnSelectedIndexChanged="Grid_Deducciones_Variables_SelectedIndexChanged">
                                                <Columns>
                                                    <asp:ButtonField ButtonType="Image" CommandName="Select"  HeaderText="Seleccionar"
                                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png" CausesValidation="false">
                                                        <ItemStyle Width="15%"  HorizontalAlign="Left"/>
                                                        <HeaderStyle HorizontalAlign="Left" Width="15%"/>
                                                    </asp:ButtonField>                                                
                                                    <asp:BoundField DataField="NO_DEDUCCION" HeaderText="No Deduccion">
                                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                    </asp:BoundField>                                                   
                                                    <asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                    </asp:BoundField>  
                                                    <%--<asp:BoundField DataField="DEPENDENCIA" HeaderText="U. Responsable">
                                                        <HeaderStyle HorizontalAlign="Left" Width="30%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="30%" />
                                                    </asp:BoundField>--%>                                                        
                                                    <asp:BoundField DataField="COMENTARIOS" HeaderText="Comentarios">
                                                        <HeaderStyle HorizontalAlign="Left" Width="40%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="40%" />
                                                    </asp:BoundField>   
                                                    <asp:BoundField DataField="NOMINA_ID" HeaderText="">
                                                        <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                    </asp:BoundField>                                                                                                                                                                                            
                                                </Columns>
                                                <SelectedRowStyle CssClass="GridSelected" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <HeaderStyle CssClass="GridHeader" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                            </asp:GridView>                                          
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
                                        * Buscar Empleado
                                    </td>
                                    <td style="text-align:left;width:30%;">     
                                        <asp:TextBox ID="Txt_Empleados" runat="server" Width="98%" OnTextChanged="Txt_Empleados_TextChanged" AutoPostBack="true"/>
                                        <cc1:TextBoxWatermarkExtender ID="TBW_Txt_Empleados" runat="server" TargetControlID="Txt_Empleados"
                                            WatermarkCssClass="watermarked2" WatermarkText="No Empleado ó Nombre"/>
                                    </td>
                                    <td style="text-align:left;width:5%;">
                                        <asp:ImageButton ID="Btn_Buscar_Empleados" runat="server" Text="Buscar" ToolTip="Buscar Empleados"
                                             ImageUrl="~/paginas/imagenes/paginas/busqueda.png" OnClick="Btn_Buscar_Empleados_Click" CausesValidation="false"/>
                                    </td>
                                    <td style="text-align:left;width:45%;">
                                        Cantidad
                                        <asp:TextBox ID="Txt_Cantidad" runat="server" Width="81%" style="text-align:right;"
                                            onblur="$('input[id$=Txt_Cantidad]').formatCurrency({colorize:true, region: 'es-MX'});"/>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Cantidad" runat="server" 
                                            TargetControlID="Txt_Cantidad" FilterType="Custom, Numbers" ValidChars=",."/>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:left;width:20%;">                          
                                        *Empleados
                                    </td>
                                    <td style="text-align:left;width:80%;" colspan="4">                          
                                        <asp:DropDownList ID="Cmb_Empleados" runat="server" Width="80%"/>
                                        <asp:Button ID="Btn_Agregar_Empleado" runat="server" Text="Agregar"  
                                                OnClick="Btn_Agregar_Empleado_Click" Width="19%" Font-Size="11px" Height="22px"
                                                ToolTip="Agregar Empleado"/>
                                    </td>
                                </tr>   
                                <tr>
                                    <td colspan="4" style="width:100%;">
                                        <hr />
                                        <br />
                                    </td>
                                </tr>       
                                <tr>
                                    <td style="width:100%;" colspan="4">
                                    
                                        <div id="Div_Resultados_Busqueda" runat="server" style="border-style:outset; width:98%; height: 350px; overflow:auto; color:White;">                  
                                    
                                            <asp:GridView ID="Grid_Empleados" runat="server" CssClass="GridView_1"
                                                 AutoGenerateColumns="False"  GridLines="Vertical"
                                                 OnRowDataBound="Grid_Empleados_RowDataBound">
                                                    <Columns>                                              
                                                        <asp:BoundField DataField="EMPLEADO_ID" HeaderText="Empleado ID">
                                                            <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="NOMBRE" HeaderText="Nombre">
                                                            <HeaderStyle HorizontalAlign="Left" Width="60%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="60%" Font-Size="11px" Font-Bold="true"/>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="CANTIDAD" HeaderText="Cantidad" DataFormatString="{0:c}">
                                                            <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                                            <ItemStyle HorizontalAlign="Center" Width="20%" Font-Size="11px" Font-Bold="true"/>
                                                        </asp:BoundField>                                                      
                                                        <asp:TemplateField HeaderText="Eliminar">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="Btn_Eliminar_Empleado" runat="server" ImageUrl="~/paginas/imagenes/gridview/grid_garbage.png" 
                                                                   OnClick="Btn_Eliminar_Empleado_Click" OnClientClick="return confirm('¿Está seguro de eliminar el Empelado seleccionado?');"
                                                                   CausesValidation="false"/>                                                    
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                                            <ItemStyle HorizontalAlign="Center" Width="20%" />                                                        
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

            <cc1:ModalPopupExtender ID="MPE_Msj" runat="server" BackgroundCssClass="popUpStyle" BehaviorID="MPE_Msj_Interno"
                PopupControlID="Pnl_Mensaje" TargetControlID="Btn_Busqueda_No_Deduccion_Variable" PopupDragHandleControlID="Pnl_Interno" 
                CancelControlID="Btn_Comodin" DropShadow="True" DynamicServicePath="" Enabled="True" />  
            <asp:Button Style="background-color: transparent; border-style:none;" ID="Btn_Comodin" runat="server" Text="" />                                                                           
        </ContentTemplate>
    </asp:UpdatePanel>     
    
    
    
<asp:Panel ID="Pnl_Mensaje" runat="server" CssClass="drag" HorizontalAlign="Center" Width="650px" 
    style="display:none;border-style:outset;border-color:Silver;background-image:url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG);background-repeat:repeat-y;">                         
    <asp:Panel ID="Pnl_Interno" runat="server" 
        style="cursor: move;background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
        <table width="99%">
            <tr>
                <td style="color:Black;font-size:12;font-weight:bold;">
                   <asp:Image ID="Image1" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                     Informacion: B&uacute;squeda de No Deducciones Variables en el Sistema
                </td>
                <td align="right" style="width:10%;">
                   <asp:ImageButton ID="Btn_Cerrar_Ventana_Emergente" CausesValidation="false" runat="server" style="cursor:pointer;" ToolTip="Cerrar Ventana"
                        ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png" OnClientClick="javascript:$find('MPE_Msj_Interno').hide(); return false;"/>  
                </td>
            </tr>
        </table>            
    </asp:Panel>                                                                          
        <div style="cursor:default;width:100%">
            <table width="100%" style="background-color:#ffffff;">
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
                        <td  style="width:20%;text-align:left;">
                           No Empleado 
                        </td>              
                        <td style="width:30%;text-align:left;">
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
                        <td  style="width:20%;text-align:left;">
                           Nombre del empleado 
                        </td>              
                        <td colspan="3" style="width:30%;text-align:left;">
                           <asp:TextBox ID="Txt_Busqueda_Nombre_Empleado" runat="server" Width="98%" MaxLength="100"/>
                           <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_Nombre_Empleado" runat="server" 
                           FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars="áéíóúÁÉÍÓÚ ñÑ"
                                TargetControlID="Txt_Busqueda_Nombre_Empleado"/>
                            <cc1:TextBoxWatermarkExtender ID="Twm_Txt_Busqueda_Nombre_Empleado" 
                                runat="server" TargetControlID ="Txt_Busqueda_Nombre_Empleado" 
                                 WatermarkText="Límite de Caractes 100" WatermarkCssClass="watermarked"/> 
                            <span id="Span1" class="watermarked"></span>                                                                                                                   
                        </td> 
                                                       
                </tr>          
                
                <tr>
                    <td style="width:20%;text-align:left;">
                        No Deducci&oacute;n
                    </td>
                    <td style="width:30%;text-align:left;">
                        <asp:TextBox ID="Txt_Busqueda_No_Deduccion" runat="server" Width="98%" MaxLength="10"/>
                        <cc1:FilteredTextBoxExtender ID="FTxt_Busqueda_No_Deduccion" 
                                runat="server" TargetControlID="Txt_Busqueda_No_Deduccion" FilterType="Numbers"/>                          
                    </td>
                    <td style="width:20%;text-align:left;">
                        Estatus
                    </td>
                    <td style="width:30%;text-align:left;">
                        <asp:DropDownList ID="Cmb_Busqueda_Estatus" runat="server" Width="100%">
                                <asp:ListItem>&lt; Seleccione &gt;</asp:ListItem>
                                <asp:ListItem>Pendiente</asp:ListItem>
                                <asp:ListItem>Aceptado</asp:ListItem>                                
                                <asp:ListItem>Rechazado</asp:ListItem>                                
                        </asp:DropDownList>                    
                    </td>                                                            
                </tr>                
               <tr>
                    <td style="width:100%" colspan="4">
                        <hr />
                    </td>
               </tr>                    
               <tr>
                    <td style="width:100%" colspan="4" align="center">
                        <asp:Button ID="Btn_Busqueda_Deducciones" runat="server" Text="Búsqueda Deducciones" OnClick="Btn_Busqueda_Deducciones_Click" CausesValidation="false"
                            style="border-style:none;background-color:White;" ToolTip="Ejecuatar la búsqueda de Deducciones en el sistema"/> 
                    </td>
               </tr>                  
               <tr>
                    <td style="width:100%" colspan="4">
                        <hr />
                    </td>
                </tr>                                            
            </table>
        </div>                 
</asp:Panel>

   
</asp:Content>


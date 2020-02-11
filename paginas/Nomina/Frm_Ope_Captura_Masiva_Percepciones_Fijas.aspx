<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Captura_Masiva_Percepciones_Fijas.aspx.cs" Inherits="paginas_Nomina_Frm_Ope_Captura_Masiva_Percepciones_Fijas" Title="Captura Masiva de Percepciones Deducciones Fijas" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
<script type="text/javascript" language="javascript">
function refresh_tabla_empleados(){ $('#tabla_empleados table > tbody > tr').remove();}
</script>

<script type="text/javascript" language="javascript">
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
<cc1:ToolkitScriptManager ID="Tsm_Captura_Masiva_Percepciones_Fijas" runat="server"  AsyncPostBackTimeout="3600"/>
<asp:UpdatePanel ID="Upnl_Captura_Masiva_Percepciones_Fijas" runat="server" UpdateMode="Always">
    <ContentTemplate>
    
        <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upnl_Captura_Masiva_Percepciones_Fijas" DisplayAfter="0">
            <ProgressTemplate>
                <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    
        <div id="Div_Captura_Masiva_Percepciones_Fijas" style="width:98%; background-color:White;">
            <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                <tr align="center">
                    <td class="label_titulo">Captura Masiva Fijos</td>
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
                                           <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button" TabIndex="17" 
                                                ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" onclick="Btn_Nuevo_Click" CausesValidation="false"/>
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" TabIndex="20"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" onclick="Btn_Salir_Click" CausesValidation="false"/>
                                      </td>
                                      <td align="right" style="width:41%;">                               
                                       </td>       
                                     </tr>         
                                  </table>                      
                                </div>
                         </td>
                     </tr>
            </table>  
        
            <table style="width:98%;">
               <tr>
                    <td colspan="4" style="width:100%;">
                        <hr />
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
                        Conceptos Fijos
                    </td>
                    <td  style="text-align:left;width:80%;" colspan="3">
                        <asp:DropDownList ID="Cmb_Percepciones" runat="server" Width="100%"/>
                    </td>   
                </tr>  
                <tr>
                    <td colspan="4" style="width:100%;">
                        <hr />
                    </td>
                </tr> 
            </table>
            
            <table style="width:98%;">
                <tr>
                    <td class="button_autorizar" style="width:100%;text-align:center;font-size:10px;cursor:default;">
                        <center>
                            <asp:RadioButtonList ID="RBtn_Tipo_Percepcion_Deduccion" runat="server" RepeatDirection="Horizontal" class="radio"
                                AutoPostBack="true" OnSelectedIndexChanged="RBtn_Tipo_Percepcion_Deduccion_SelectedIndexChanged"
                                Font-Size="11px">
                                <asp:ListItem>PERCEPCION</asp:ListItem>
                                <asp:ListItem>DEDUCCION</asp:ListItem>
                            </asp:RadioButtonList>
                        </center>
                    </td>
                </tr>
            </table>
            
            <table style="width:98%;">
                <tr>
                    <td style="color:White; border-style: outset;width:100%;cursor:hand;padding:2px 4px 2px 4px;background:url(../imagenes/paginas/titleBackground.png) repeat-x top;background-color:#A9D0F5;"
                        colspan="4" align="center">
                        <cc1:AsyncFileUpload ID="AFU_Cargar_Archivo_Fijos" runat="server" 
                            Width="600px" ThrobberID="Lbl_Trobber"/>
                        <asp:Label ID="Lbl_Trobber" runat="server" >
                            <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                            <div  class="processMessage" id="div_progress">
                                <img alt="" src="../Imagenes/paginas/Indicador_Presidencia.gif" />
                            </div>
                        </asp:Label>
                   </td>
                </tr>
                <tr>
                   <td style="width:50%;" colspan="2">
                        <asp:Button ID="Btn_Cargar_Empleados" runat="server" Text="Cargar" 
                           OnClick="Btn_Cargar_Empleados_Click" CssClass="button_autorizar" Width="100%"/>
                   </td>
                   <td style="width:50%;" colspan="2">
                        <asp:Button ID="Btn_Limpiar_Empleados" runat="server" Text="Limpiar" OnClientClick="javascript:refresh_tabla_empleados();return false;"
                            OnClick="Btn_Limpiar_Empleados_Click" CssClass="button_autorizar" Width="100%"/>
                    </td>
                </tr>
               <tr>
                    <td colspan="4" style="width:100%;">
                    </td>
                </tr>
            </table>
            
             <table style="width:98%;">
                <tr>
                    <td style="width:100%;" colspan="4" id="tabla_empleados">
                        <div >
                            <table style="width:98%;" border="0">
                                <th style="width:60%;text-align:left;" class="GridHeader_Nested">Nombre</th>
                                <th style="width:20%;text-align:center;" class="GridHeader_Nested">Cantidad</th>
                                <th style="width:20%;text-align:center;" class="GridHeader_Nested">Eliminar</th>
                            </table>
                        </div>
                        <div id="Div_Resultados_Busqueda" runat="server" style="border-style:outset; width:98%; height: 350px; overflow:auto; color:White;">                  
                            <asp:GridView ID="Grid_Empleados" runat="server" CssClass="GridView_1"
                                 AutoGenerateColumns="False"  GridLines="Vertical" ShowHeader="false" ShowFooter="true"
                                 OnRowDataBound="Grid_Empleados_RowDataBound">
                                 
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <PagerStyle CssClass="GridHeader" />
                                    <HeaderStyle CssClass="GridHeader" />
                                    <FooterStyle CssClass="GridHeader_Nested" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                    
                                    <Columns>                                              
                                        <asp:BoundField DataField="EMPLEADO_ID" HeaderText="Empleado ID">
                                            <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                            <ItemStyle HorizontalAlign="Left" Width="0%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NOMBRE" HeaderText="Nombre">
                                            <HeaderStyle HorizontalAlign="Left" Width="60%" />
                                            <ItemStyle HorizontalAlign="Left" Width="60%" Font-Size="11px" Font-Bold="true"/>
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Retención Cat.">
                                            <ItemTemplate>
                                                <asp:Label ID ="Lbl_Cantidad" runat="server" Text='<%#Eval("CANTIDAD")%>' />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <%# HttpUtility.HtmlDecode("&nbsp;&nbsp;<b style='font-family=Courier New; font-size=13px;'>TOTAL=$") + String.Format("{0:c}", Obtener_Total() + "</b>")%>
                                            </FooterTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="100px" />
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />                                            
                                        </asp:TemplateField>                                                       
                                        <asp:TemplateField HeaderText="Eliminar">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="Btn_Eliminar_Empleado" runat="server" ImageUrl="~/paginas/imagenes/gridview/grid_garbage.png" 
                                                   OnClick="Btn_Eliminar_Empleado_Click" OnClientClick="return confirm('¿Está seguro de eliminar el Empleado seleccionado?');"
                                                   CausesValidation="false"/>                                                    
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                            <ItemStyle HorizontalAlign="Center" Width="20%" />                                                        
                                        </asp:TemplateField>                                                         
                                    </Columns>
                                </asp:GridView>    
                            </div>                                
                        </td>
                    </tr>                                              
                </table>   
        </div>
        
        <asp:HiddenField ID="HTxt_Referencia" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>


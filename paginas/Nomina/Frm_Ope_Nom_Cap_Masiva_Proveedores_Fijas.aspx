<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Nom_Cap_Masiva_Proveedores_Fijas.aspx.cs" Inherits="paginas_Frm_Ope_Nom_Cap_Masiva_Proveedores_Fijas" Title="Captura Masiva de Conceptos Fijos Por Proveedor" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
    <link href="../../easyui/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../../easyui/themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="../../easyui/jquery.easyui.min.js" type="text/javascript"></script>
    
<script type="text/javascript">
function pageLoad(){
    $('#contenedor_empleados table > tbody > tr:not(:has(table, th))').click(function(e){
        var $row = $(this);
        var texto = "Empleado: " + $('td', $row).eq(0).text() + "<br />" +
                    "Deduccion: " + $('td', $row).eq(1).text() + "<br />" +
                    "Cantidad: [" + $('td', $row).eq(2).text() + "]<br />" +
                    "Importe: [" + $('td', $row).eq(3).text() + "]<br />" +
                    "Saldo: [" + $('td', $row).eq(4).text() + "]<br />" +
                    "Abonada: [" + $('td', $row).eq(5).text() + "]<br />";

			$.messager.show({
				title:'Información',
				msg:texto,
				showType:'fade',
				width:400,
				height:180
			});
        });
        
        $('select[id$=Cmb_Proveedores]').bind("change", function(){
            var Opcion_Seleccionada = $('select[id$=Cmb_Proveedores] :selected').val();
            if(Opcion_Seleccionada == ""){
                $('#archivo').attr('disabled', 'disabled');
            }else{
                $('#archivo').removeAttr('disabled');
            }
            
            $('#contenedor_empleados table > tbody > tr').remove();
        });
        
        var Opcion_Seleccionada = $('select[id$=Cmb_Proveedores] :selected').val();
        if(Opcion_Seleccionada == ""){
            $('#archivo').attr('disabled', 'disabled');
        }else {
            $('#archivo').removeAttr('disabled');
        }
}
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
    <cc1:ToolkitScriptManager ID="Tsm_Generar_Nomina" runat="server"  AsyncPostBackTimeout="3600"/>
    <asp:UpdatePanel ID="UPnl_Captura_Masiva_Proveedores_Fijas" runat="server" UpdateMode="Always">
        <ContentTemplate>
        
        <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="UPnl_Captura_Masiva_Proveedores_Fijas" DisplayAfter="0">
            <ProgressTemplate>
                <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        
        <div id="Div_Captura_Masiva_Proveedores_Fijas" style="background-color:White; width:98%;">
            <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                <tr align="center">
                    <td class="label_titulo">Captura Deducciones Por Proveedor</td>
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
                    <td style="width:20%; text-align:left;">
                        Proveedor
                    </td>
                    <td style="width:80%; text-align:left;" colspan="3">
                        <asp:DropDownList ID="Cmb_Proveedores" runat="server" Width="100%"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="width:100%;">
                        <hr />
                    </td>
                </tr> 
            </table>
            
            <table style="width:98%;">
                <tr id="archivo">
                    <td style="color:White; border-style: outset;width:100%;cursor:hand;padding:2px 4px 2px 4px;background:url(../imagenes/paginas/titleBackground.png) repeat-x top;background-color:#A9D0F5;"
                        colspan="4" align="center">
                        <cc1:AsyncFileUpload ID="AFU_Cargar_Archivo_Fijos" runat="server" class="archivo"
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
                        <asp:Button ID="Btn_Limpiar_Empleados" runat="server" Text="Limpiar" 
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
                    <td style="width:100%;" colspan="4">
                        <div id="contenedor_empleados" style="overflow:auto;height:400px;width:815px;vertical-align:top;border-style:outset;border-color:Silver;" >
                            <asp:GridView ID="Grid_Empleados" runat="server" CssClass="GridView_1"
                                 AutoGenerateColumns="False"  GridLines="Both" ShowHeader="true" ShowFooter="true"
                                 OnRowDataBound="Grid_Empleados_RowDataBound" Width="1100px">
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <PagerStyle CssClass="GridHeader" />
                                    <HeaderStyle CssClass="GridHeader" />
                                    <FooterStyle CssClass="GridHeader" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                    <Columns>                                              
                                        <asp:BoundField DataField="EMPLEADO_ID" HeaderText="">
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NOMBRE" HeaderText="Nombre">
                                            <HeaderStyle HorizontalAlign="Left" Width="300px" />
                                            <ItemStyle HorizontalAlign="Left" Width="300px" Font-Size="11px" Font-Bold="true"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NOMBRE_DEDUCCION" HeaderText="Deducción">
                                            <HeaderStyle HorizontalAlign="Left" Width="300px" />
                                            <ItemStyle HorizontalAlign="Left" Width="300px" Font-Size="10px" Font-Bold="true"/>
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Retención Cat.">
                                            <ItemTemplate>
                                                <asp:Label ID ="Lbl_Cantidad3" runat="server" Text='<%#Eval("CANTIDAD")%>' style="font-family:Courier New; font-size:small; font-weight:bold;"/>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <%# HttpUtility.HtmlDecode("&nbsp;&nbsp;<b style='font-family=Courier New; font-size=13px;'>TOTAL=$") + String.Format("{0:c}", Obtener_Total(3) + "</b>")%>
                                            </FooterTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="100px" />
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />                                            
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Importe">
                                            <ItemTemplate>
                                                <asp:Label ID ="Lbl_Cantidad4" runat="server" Text='<%#Eval("IMPORTE")%>' style="font-family:Courier New; font-size:small; font-weight:bold;"/>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <%# HttpUtility.HtmlDecode("&nbsp;&nbsp;<b style='font-family=Courier New; font-size=13px;'>TOTAL=$") + String.Format("{0:c}", Obtener_Total(4) + "</b>")%>
                                            </FooterTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="100px" />
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />                                            
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Saldo">
                                            <ItemTemplate>
                                                <asp:Label ID ="Lbl_Cantidad5" runat="server" Text='<%# Eval("SALDO")%>' style="font-family:Courier New; font-size:small; font-weight:bold;"/>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <%# HttpUtility.HtmlDecode("&nbsp;&nbsp;<b style='font-family=Courier New; font-size=13px;'>TOTAL=$") + String.Format("{0:c}", Obtener_Total(5) + "</b>")%>
                                            </FooterTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="100px" />
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />                                            
                                        </asp:TemplateField>                                                       
                                         <asp:TemplateField HeaderText="$ Retenida">
                                            <ItemTemplate>
                                                <asp:Label ID ="Lbl_Cantidad6" runat="server" Text='<%#Eval("CANTIDAD_RETENIDA")%>' style="font-family:Courier New; font-size:small; font-weight:bold;"/>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <%# HttpUtility.HtmlDecode("&nbsp;&nbsp;<b style='font-family=Courier New; font-size=13px;'>TOTAL=$") + String.Format("{0:c}", Obtener_Total(6) + "</b>")%>
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
                                            <HeaderStyle HorizontalAlign="Center" Width="30px" />
                                            <ItemStyle HorizontalAlign="Center" Width="30px" />                                                        
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="DEDUCCION_ID" HeaderText="">
                                            <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                            <ItemStyle HorizontalAlign="Left" Width="0%" />
                                        </asp:BoundField>                                                         
                                    </Columns>
                                </asp:GridView>
                            </div>                                    
                        </td>
                    </tr>                                              
                </table> 
        </div>
        
            <asp:HiddenField ID="HTxt_Referencia" runat="server" />
            
        <div id="dialog" title="Order Details">
            <div id="tbl"></div>
        </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


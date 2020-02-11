<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Nom_Faltas_Empleados.aspx.cs" Inherits="paginas_Nomina_Frm_Ope_Nom_Faltas_Empleados" Title="Faltas de los Empleados" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
    <link href="../../easyui/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../../easyui/themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="../../easyui/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="../../javascript/Js_Ope_Nom_Faltas_Empleados.js" type="text/javascript"></script>
    
    <script type="text/javascript" language="javascript">
         function pageLoad(){
            $('input[id*=Txt_Busqueda_Falta_Empleado]').click(function(){ $(this).val(''); });
                 
            $('input[id$=Txt_Busqueda_Falta_Empleado]').bind('dblclick', function(){
                $.ajax({
                    url: "Frm_Informacion_Empleado.aspx?tabla=EMPLEADOS&opcion=consultar_empleados&no_empleado=" + $(this).val(),
                    type:'POST',
                    async: false,
                    cache: false,
                    dataType:'json',
                    success: function (Datos) {
                         if (Datos != null) {  
                            var texto = "";
                             $.each(Datos.EMPLEADOS, function (Contador, Elemento) {                                  
                                texto = texto + "Empleado: " + Elemento.NO_EMPLEADO + "<br />" +
                                        "Nombre: " + Elemento.EMPLEADO + "<br />" +
                                        "Unidad Responsable: " + Elemento.UR + "<br />" +
                                        "Salario Diario: $" + Elemento.SALARIO_DIARIO+ "<br />";

                                 texto = texto + "<br /><br />";
                             });
	                            $.messager.show({
		                            title:'Información Empleado',
		                            msg:texto,
		                            showType:'fade',
		                            width:400,
		                            height:180
	                            });   
                         }
                         else {
                             alert("El empleado no existe en el sistema.");
                         }      
                    }        
                });
            });    
         }

         Inicializar_Eventos_Faltas_Empleados(); 
         
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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <cc1:ToolkitScriptManager ID="Tsm_Faltas_Empleados" runat="server"  AsyncPostBackTimeout="36000"  EnableScriptGlobalization = "true" EnableScriptLocalization = "True"/>
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
        
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            
             <div id="Div_Faltas_Empleados" style="background-color:#ffffff; width:100%; height:100%;">
            
                    <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                        <tr align="center">
                            <td class="label_titulo">Captura de Faltas de los Empleados</td>
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
                                                        OnClientClick="return confirm('¿Está seguro de eliminar la Falta Seleccionada seleccionado?');"/>
                                                    <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" onclick="Btn_Salir_Click"/>
                                              </td>
                                              <td align="right" style="width:41%;">
                                                <table style="width:100%;height:28px;">
                                                    <tr>
                                                        <td style="vertical-align:middle;text-align:right;width:20%;">B&uacute;squeda:</td>
                                                        <td style="width:55%;">
                                                            <asp:TextBox ID="Txt_Busqueda_Falta_Empleado" runat="server" MaxLength="100" Width="200px" AutoPostBack="true"
                                                                OnTextChanged="Txt_Busqueda_Falta_Empleado_TextChanged"/>
                                                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Falta_Empleado" runat="server" WatermarkCssClass="watermarked"
                                                                WatermarkText="Nombre o No Empleado" TargetControlID="Txt_Busqueda_Falta_Empleado" />
                                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Falta_Empleado" runat="server" 
                                                                TargetControlID="Txt_Busqueda_Falta_Empleado" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                                                ValidChars="ÑñáéíóúÁÉÍÓÚ. " />
                                                        </td>
                                                        <td style="vertical-align:middle;width:5%;" >
                                                            <asp:ImageButton ID="Btn_Busqueda_Faltas_Empleado" runat="server" ToolTip="Consultar"
                                                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" OnClick="Btn_Busqueda_Faltas_Empleado_Click"/>                                        
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
                        <td style="text-align:left;width:20%;vertical-align:top;display:none">
                            No Falta
                        </td>
                        <td  style="text-align:left;width:30%; display:none">
                            <asp:TextBox ID="Txt_No_Falta_Empleado" runat="server" Width="98%"/>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;vertical-align:top;">                            
                        </td>
                        <td  style="text-align:left;width:30%;">                            
                        </td>
                        <td style="text-align:left;width:20%;">
                            &nbsp;&nbsp;Cantidad Descontar                                                       
                        </td>
                        <td  style="text-align:left;width:30%;vertical-align:top;">
                            <asp:TextBox ID="Txt_Cantidad_Descontar" runat="server" Width="97%" CssClass="watermarked" ReadOnly="true" Font-Size="15"
                                style="background-color:Transparent; border-style:solid; border-width:2px; border-color:Blue;text-align:right;"/>
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
                            *U. Responsable
                        </td>
                        <td style="text-align:left;width:80%;" colspan="3">     
                            <asp:DropDownList ID="Cmb_Dependencia" runat="server" Width="100%" Enabled="false"
                                onselectedindexchanged="Cmb_Dependencia_SelectedIndexChanged" AutoPostBack="true"/>
                        </td>                      
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            *Empleados
                        </td>
                        <td style="text-align:left;width:80%;" colspan="3">     
                            <asp:DropDownList ID="Cmb_Empleados" runat="server" Width="100%" 
                                onselectedindexchanged="Cmb_Empleados_SelectedIndexChanged" AutoPostBack="true"/>
                        </td>                      
                    </tr>                    
                   <tr>
                        <td style="text-align:left;width:20%;">                          
                            *Tipo Falta
                        </td>
                        <td style="text-align:left;width:30%;">     
                            <asp:DropDownList ID="Cmb_Tipo_Falta_Empleado" runat="server" Width="100%">
                                <asp:ListItem>&lt; Seleccione &gt;</asp:ListItem>
                                <asp:ListItem>INASISTENCIA</asp:ListItem>
                                <asp:ListItem>JUSTIFICADA</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="text-align:left;width:20%;">
                            *Fecha Falta
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Fecha_Falta_Empleado" runat="server"  Width="85%"
                                onblur="$('input[id$=Txt_Fecha_Falta_Empleado]').filter(function(){if(!this.value.match(/^(0[1-9]|[12][0-9]|3[01])(0[1-9]|1[012])(19|20)\d\d$/))$(this).val('');});"/>
                            <cc1:FilteredTextBoxExtender ID="Txt_Fecha_Falta_Empleado_FilteredTextBoxExtender" 
                                runat="server" TargetControlID="Txt_Fecha_Falta_Empleado" FilterType="Numbers" />
                            <cc1:CalendarExtender ID="Txt_Fecha_Falta_Empleado_CalendarExtender" runat="server" 
                                TargetControlID="Txt_Fecha_Falta_Empleado" PopupButtonID="Btn_Fecha_Falta_Empleado" Format="ddMMyyyy"/>
                            <asp:ImageButton ID="Btn_Fecha_Falta_Empleado" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                ToolTip="Seleccione la Fecha de la Falta del Empleado"/>                              
                        </td>       
                    </tr>          
                    <tr style="display:none;">
                        <td style="text-align:left;width:20%;">
                            *Retardo
                        </td>
                        <td  style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Retardo_Empleado" runat="server" Width="100%"
                                AutoPostBack="true" 
                                onselectedindexchanged="Cmb_Retardo_Empleado_SelectedIndexChanged">
                                <asp:ListItem>&lt; Seleccione &gt;</asp:ListItem>
                                <asp:ListItem>SI</asp:ListItem>
                                <asp:ListItem>NO</asp:ListItem>
                            </asp:DropDownList>                        
                        </td>
                        <td style="text-align:left;width:20%;">
                            *Cantidad Minutos                           
                        </td>
                        <td  style="text-align:left;width:30%;">                            
                            <asp:TextBox ID="Txt_Cantidad_Minutos_Retardo_Empleado" runat="server" Width="98%" MaxLength="9"
                                AutoPostBack="true" OnTextChanged="Txt_Cantidad_Minutos_Retardo_Empleado_TextChanged"/>
                            <cc1:FilteredTextBoxExtender ID="FTxt_Cantidad_Minutos_Retardo_Empleado" runat="server"  
                                TargetControlID="Txt_Cantidad_Minutos_Retardo_Empleado" FilterType="Numbers, Custom" ValidChars="."/>                              
                        </td>                        
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;vertical-align:top;">
                            Comentarios
                        </td>
                        <td  style="text-align:left;width:80%;" colspan="3">
                            <asp:TextBox ID="Txt_Comentarios" runat="server" Width="99%" MaxLength="100" TextMode="MultiLine" Wrap="true"
                                Height="70px"/>
                            <cc1:FilteredTextBoxExtender ID="FTxt_Comentarios" runat="server"  TargetControlID="Txt_Comentarios"
                                FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars="áéíóúÁÉÍÓÚ ñÑ"/>    
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Comentarios" runat="server" TargetControlID ="Txt_Comentarios" 
                                WatermarkText="Límite de Caractes 250" WatermarkCssClass="watermarked"/> 
                            <span id="Contador_Caracteres_Comentarios" class="watermarked"></span>                                                     
                        </td>
                    </tr>                                                                                                                                  
                </table>
                
                <div style="overflow:auto;height:400px;width:99%;vertical-align:top;border-style:outset;color:White;" >
                    <table width="98%">
                        <tr align="center">
                            <td colspan="4">
                                <asp:GridView ID="Grid_Faltas_Empleado" runat="server" CssClass="GridView_1"
                                     AutoGenerateColumns="False"  GridLines="None"
                                    onpageindexchanging="Grid_Faltas_Empleado_PageIndexChanging" 
                                    onselectedindexchanged="Grid_Faltas_Empleado_SelectedIndexChanged"
                                    AllowSorting="True" OnSorting="Grid_Programa_Sorting" HeaderStyle-CssClass="tblHead">
                                         <Columns>
                                             <asp:ButtonField ButtonType="Image" CommandName="Select"  HeaderText=""
                                                 ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                 <ItemStyle Width="5%" HorizontalAlign="Left"/>
                                             </asp:ButtonField>
                                             <asp:BoundField DataField="NO_FALTA" HeaderText="No Falta" SortExpression="NO_FALTA">
                                                 <HeaderStyle HorizontalAlign="Left" Width="15%"  Font-Bold="true" Font-Size="X-Small"/>
                                                 <ItemStyle HorizontalAlign="Left" Width="15%" Font-Bold="true" Font-Size="X-Small"/>
                                             </asp:BoundField>
                                             <asp:BoundField DataField="EMPLEADO_ID" HeaderText="">
                                                 <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                 <ItemStyle HorizontalAlign="Left" Width="0%" />
                                             </asp:BoundField>
                                             <asp:BoundField DataField="DEPENDENCIA" HeaderText="U. Responsable" SortExpression="DEPENDENCIA">
                                                 <HeaderStyle HorizontalAlign="Left" Width="25%" Font-Bold="true" Font-Size="X-Small"/>
                                                 <ItemStyle HorizontalAlign="Left" Width="25%" Font-Bold="true" Font-Size="X-Small"/>
                                             </asp:BoundField>
                                             <asp:BoundField DataField="FECHA" HeaderText="Fecha" DataFormatString="{0:dd/MMM/yyyy}"  SortExpression="FECHA">
                                                 <HeaderStyle HorizontalAlign="Left" Width="15%" Font-Bold="true" Font-Size="X-Small"/>
                                                 <ItemStyle HorizontalAlign="Left" Width="15%" Font-Bold="true" Font-Size="X-Small"/>
                                             </asp:BoundField>                                                                                                                           
                                             <asp:BoundField DataField="TIPO_FALTA" HeaderText="Tipo Falta" SortExpression="TIPO_FALTA">
                                                  <HeaderStyle HorizontalAlign="Left" Width="20%" Font-Bold="true" Font-Size="X-Small"/>
                                                  <ItemStyle HorizontalAlign="Left" Width="20%" Font-Bold="true" Font-Size="X-Small"/>
                                             </asp:BoundField>
                                             <asp:BoundField DataField="RETARDO" HeaderText="Retardo" SortExpression="RETARDO">
                                                  <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                                  <ItemStyle HorizontalAlign="Center" Width="20%" />
                                             </asp:BoundField>
                                             <asp:BoundField DataField="CANTIDAD" HeaderText="">
                                                  <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                  <ItemStyle HorizontalAlign="Left" Width="20%" />
                                             </asp:BoundField>                                                                                  
                                             <asp:BoundField DataField="COMENTARIOS" HeaderText="Comentarios">
                                                  <HeaderStyle HorizontalAlign="Left" Width="50%" />
                                                  <ItemStyle HorizontalAlign="Left" Width="50%" />
                                             </asp:BoundField>
                                             <asp:BoundField DataField="NOMINA_ID" HeaderText="">
                                                 <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                 <ItemStyle HorizontalAlign="Left" Width="0%" />
                                             </asp:BoundField>
                                             <asp:BoundField DataField="NO_NOMINA" HeaderText="">
                                                 <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                 <ItemStyle HorizontalAlign="Left" Width="0%" />
                                             </asp:BoundField>                                               
                                         </Columns>
                                         <SelectedRowStyle CssClass="GridSelected" />
                                         <PagerStyle CssClass="GridHeader" />
                                         
                                         <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
            </div>                                               
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>           
</asp:Content>
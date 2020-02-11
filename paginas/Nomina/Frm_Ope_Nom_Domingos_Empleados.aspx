<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Nom_Domingos_Empleados.aspx.cs" Inherits="paginas_Nomina_Frm_Ope_Nom_Domingos_Empleados" Title="Domingos Trabajados" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
    <script src="../../javascript/Js_Ope_Nom_Domingos.js" type="text/javascript"></script>
    <link href="../../easyui/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../../easyui/themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="../../easyui/jquery.easyui.min.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">
        function calendarShown(sender, args){
            sender._popupBehavior._element.style.zIndex = 10000005;
        }
        function Limpiar_Ctlr(){
            document.getElementById("<%=Txt_Busqueda_No_Domingo.ClientID %>").value="";
            document.getElementById("<%=Cmb_Busqueda_Estatus.ClientID %>").value="";
            document.getElementById("<%=Txt_Busqueda_Fecha_Inicio.ClientID %>").value="";
            document.getElementById("<%=Txt_Busqueda_Fecha_Fin.ClientID %>").value="";
            document.getElementById("<%=Txt_Busqueda_No_Empleado.ClientID %>").value="";
            return false;
        }
        
        function pageLoad(sender, args) {
            $('textarea[id*=Txt_Comentarios_Domingo_Trabajado]').keyup(function() {var Caracteres =  $(this).val().length; if (Caracteres > 250) {this.value = this.value.substring(0, 250); Caracteres =  $(this).val().length;  $(this).css("background-color", "Yellow");$(this).css("color", "Red");}else{$(this).css("background-color", "White");$(this).css("color", "Black");}$('#Contador_Caracteres_Comentarios').text('Carácteres Ingresados [ ' + Caracteres + ' ]/[ 250 ]');});
            
            $('input[id$=Txt_Empleado_Domingo_Trabajado]').bind('dblclick', function(){
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
                                        "Unidad Responsable: <input  onclick='Seleccionar_Unidad_Responsable(this);' onmouseover='a(this);' id='" + Elemento.DEPENDENCIA_ID + "' type='button' value='" + Elemento.UR + 
                                        "' style='background:url(../imagenes/paginas/arrowbullet.png) no-repeat center left; font:bold 12px normal 11px Tahoma; color:Black; display:block;  width:350px; padding:2px 0; padding-left:19px; text-decoration:none; border-style:outset; cursor:hand;'/><br />";
                                        
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
            
       $('input[id$=Txt_Busqueda_No_Empleado]').live("blur", function(){
            if(isNumber($(this).val())){
                var Ceros = "";
                if($(this).val() != undefined){
                    if($(this).val() != ''){
                        for(i=0; i<(6-$(this).val().length); i++){
                            Ceros += '0';
                        }
                        $(this).val(Ceros + $(this).val());
                        Ceros = "";
                    }else $(this).val('');
                }
            }
        });    
        
       $('input[id$=Txt_Busqueda_No_Domingo]').live("blur", function(){
            if(isNumber($(this).val())){
                var Ceros = "";
                if($(this).val() != undefined){
                    if($(this).val() != ''){
                        for(i=0; i<(10-$(this).val().length); i++){
                            Ceros += '0';
                        }
                        $(this).val(Ceros + $(this).val());
                        Ceros = "";
                    }else $(this).val('');
                }
            }
        });     
        
        $('input[id$=Btn_Busqueda_Domingos_Trabajados]').hover(
            function(e){
                e.preventDefault();
                $(this).css("background-color", "#2F4E7D");
                $(this).css("color", "#FFFFFF");
            },
            function(e){
                e.preventDefault();
                 $(this).css("background-color", "#f5f5f5");
                 $(this).css("color", "#565656");
            }
        );       
        }
        
    function isNumber(n) {   return !isNaN(parseFloat(n)) && isFinite(n); }            
    function Seleccionar_Unidad_Responsable(UR){$('select[id$=Cmb_Dependencia_Domingo_Trabajado]').val(UR.id);}  
    function a(Ctlr){
        $("#" + Ctlr.id).hover(
            function(e){
                e.preventDefault();
                $(this).css("background", 'url(../imagenes/paginas/glossyback.gif) repeat-x bottom left');
                $(this).css("font","bold 12px 'Lucida Grande', 'Trebuchet MS', Verdana, Helvetica, sans-serif");
                $(this).css("color","white");
                $(this).css("display","block");
//                $(this).css("position","relative");
                $(this).css("width","350px");
                $(this).css("padding","2px 0");
                $(this).css("padding-left","19px");
                $(this).css("text-decoration","none");
                $(this).css("border-style","none");
            },
            function(e){
                e.preventDefault();
                $(this).css("background", 'url(../imagenes/paginas/arrowbullet.png) no-repeat center left');
                $(this).css("font","bold 12px 'normal 11px 'Tahoma'");
                $(this).css("color","black");
                $(this).css("display","block");
                $(this).css("font-weight","normal");
                $(this).css("padding","2px 0");
                $(this).css("width","350px");
                $(this).css("padding-left","19px");
                $(this).css("text-decoration","none");
                $(this).css("border-style","outset");
                $(this).css("cursor","hand");
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

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">  
    <cc1:ToolkitScriptManager ID="Tsm_Domingos_Trabajados" runat="server"  AsyncPostBackTimeout="36000"  EnableScriptGlobalization = "true" EnableScriptLocalization = "True"/>

    <asp:UpdatePanel ID="Upd_Panel" runat="server"> 
        <ContentTemplate>
        
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div class="processMessage" id="div_progress"><img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>                
            </asp:UpdateProgress>
            
            <div id="Div_Domingos_Trabajados" style="background-color:#ffffff; width:100%; height:100%;">
            
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="6" class="label_titulo">
                            Catálogo Prima Dominical
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">&nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
                </table>
                
                <table class="barra_busqueda" style="border-style:none; width:98%;" width="98%">    
                    <tr  align="right">
                        <td colspan="5" style="width:50%;vertical-align:top;border-style:none;" align = "left">
                           <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo"
                                CssClass="Img_Button" 
                                ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"
                                onclick="Btn_Nuevo_Click" />
                            <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar"
                                CssClass="Img_Button" 
                                ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                onclick="Btn_Modificar_Click" />
                            <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar"
                                CssClass="Img_Button" onclick="Btn_Eliminar_Click"
                                ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"
                                OnClientClick="return confirm('¿Está seguro de eliminar el Domingo seleccionado?');"/>
                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio"
                                CssClass="Img_Button" onclick="Btn_Salir_Click"
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" />
                        </td>
                        <td style="width:50%; vertical-align:top;">
                           <asp:ImageButton ID="Btn_Busqueda_Domingo_Trabajado" runat="server" ToolTip="Avanzada"  
                                ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="24px" Width="24px"
                                OnClientClick="javascript:$find('Busqueda_Empleados').show();return false;" CausesValidation="false"/>                                 
                        </td>
                    </tr>
                </table>
                
                <table width="98%">    
                    <tr style="display:none">
                        <td style="text-align:left;width:20%;">
                            No Domingo
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_No_Domingo" runat="server" ReadOnly="true" Width="98%"/>
                        </td>
                        <td style="text-align:left;width:20%;">
                 
                        </td>
                        <td style="text-align:left;width:30%;">
                            
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
                            *Fecha
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Fecha_Domingo_Trabajado" Width="98%" runat="server" MaxLength="11" 
                                onblur="$('input[id$=Txt_Fecha_Domingo_Trabajado]').filter(function(){if(!this.value.match(/^(0[1-9]|[12][0-9]|3[01])(0[1-9]|1[012])(19|20)\d\d$/))$(this).val('');});"/>
                            <%--<cc1:FilteredTextBoxExtender ID="FTE_Fecha_Domingo_Trabajado" runat="server" 
                                TargetControlID="Txt_Fecha_Domingo_Trabajado" FilterType="UppercaseLetters, LowercaseLetters, Custom, Numbers" ValidChars="/"/>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Fecha_Domingo_Trabajado" runat="server" 
                                TargetControlID="Txt_Fecha_Domingo_Trabajado" WatermarkCssClass="watermarked" WatermarkText="Dia/Mes/Año" />
                            <cc1:CalendarExtender ID="CE_Fecha_Domingo" runat="server" Format="dd/MMM/yyyy"
                                TargetControlID="Txt_Fecha_Domingo_Trabajado" PopupButtonID="Btn_Fecha_Domingo_Trabajado" />--%>
                            <asp:ImageButton ID="Btn_Fecha_Domingo_Trabajado" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                ToolTip="Seleccione la Fecha" style="display:none;"/>
                        </td>
                        <td style="text-align:left;width:20%;">
                            &nbsp;&nbsp;Estatus
                        </td>
                        <td  style="text-align:left;width:30%;">   
                            <asp:DropDownList ID="Cmb_Estatus_Domingo_Trabajado" runat="server" Width="100%">
                                <asp:ListItem>&lt; - Seleccione - &gt;</asp:ListItem>
                                <asp:ListItem>Pendiente</asp:ListItem>   
                                <asp:ListItem>Aceptado</asp:ListItem>                                
                                <asp:ListItem>Rechazado</asp:ListItem>  
                            </asp:DropDownList>
                        </td>                          
                    </tr>                                   
                    <tr>
                        <td style="width:10%">Comentarios</td>
                        <td style="width:90%" colspan="5">
                            <asp:TextBox ID="Txt_Comentarios_Domingo_Trabajado" runat="server" TextMode="MultiLine" Width="99.5%"
                                Height="60px" MaxLength="1" Wrap="true"/>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Comentarios_Domingo_Trabajado" runat="server" TargetControlID="Txt_Comentarios_Domingo_Trabajado" 
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. ">
                            </cc1:FilteredTextBoxExtender>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Comentarios_Domingo_Trabajado" runat="server" TargetControlID="Txt_Comentarios_Domingo_Trabajado" 
                                WatermarkText="Límite de Caractes 250" WatermarkCssClass="watermarked">
                            </cc1:TextBoxWatermarkExtender>
                            <span id="Contador_Caracteres_Comentarios" class="watermarked"></span>    
                        </td>
                    </tr>
                    <tr>
                        <td style="width:100%;vertical-align:top;" colspan="6" align="right">
                            <asp:HyperLink ID="Btn_Autorizacion_Domingo_Trabajado" runat="server" Text="Autorizacion Domingo Trabajado" Visible="false" ForeColor="#0000FF"/>
                        </td>
                    </tr>
                </table>
                
                <br />
                                
                <cc1:TabContainer ID="Tab_Dias_Domingos_Trabajados" runat="server" 
                    ActiveTabIndex="0" Width="98%" CssClass="Tab">
                    <cc1:TabPanel ID="Tab_Dia_Domingo" runat="server" HeaderText="Dias Domingos">
                        <HeaderTemplate>Dias Domingos</HeaderTemplate>
                        <ContentTemplate>
                            <div style="overflow:auto;height:400px;width:99%;vertical-align:top;border-style: outset;color:White;" >
                                <asp:GridView ID="Grid_Domingos_Trabajados" runat="server" 
                                    CssClass="GridView_1" Width="98%"  
                                    AutoGenerateColumns="False"  GridLines="None" 
                                    onpageindexchanging="Grid_Domingos_Trabajados_PageIndexChanging" 
                                    onselectedindexchanged="Grid_Domingos_Trabajados_SelectedIndexChanged">
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select"  HeaderText="Seleccionar"
                                            ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                            <ItemStyle Width="15%"  HorizontalAlign="Left"/>
                                            <HeaderStyle HorizontalAlign="Left" Width="15%"/>
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="NO_DOMINGO" HeaderText="No Domingo">
                                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                            <ItemStyle HorizontalAlign="Left" Width="15%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FECHA" HeaderText="Fecha" DataFormatString="{0:dd/MMM/yyyy}">
                                             <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                             <ItemStyle HorizontalAlign="Left" Width="15%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DEPENDENCIA_ID" HeaderText="U. Responsable ID">
                                            <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                            <ItemStyle HorizontalAlign="Left" Width="0%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Dependencia" HeaderText="U. Responsable">
                                            <HeaderStyle HorizontalAlign="Left" Width="35%" />
                                            <ItemStyle HorizontalAlign="Left" Width="35%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                            <ItemStyle HorizontalAlign="Left" Width="15%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="COMENTARIOS" HeaderText="Comentarios">
                                            <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                            <ItemStyle HorizontalAlign="Left" Width="0%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NOMINA_ID" HeaderText="">
                                            <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                            <ItemStyle HorizontalAlign="Left" Width="0%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NO_NOMINA" HeaderText="">
                                            <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                            <ItemStyle HorizontalAlign="Left" Width="0%" />
                                        </asp:BoundField>   
                                        
                                          
                                        <asp:TemplateField HeaderText="Reporte">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="Btn_Reporte_Prima_Dominical" runat="server" 
                                                       ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png" Width="24px"
                                                       OnClick="Btn_Reporte_Prima_Dominical_Click" 
                                                       CausesValidation="false"/>                                                    
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" Width="1%" Height="1%"/>
                                                <ItemStyle HorizontalAlign="Center" Width="1%" Height="1%"/>                                                        
                                            </asp:TemplateField>      
                                                                                  
                                    </Columns>
                                    <SelectedRowStyle CssClass="GridSelected" /><PagerStyle CssClass="GridHeader" /><HeaderStyle CssClass="GridHeader" /><AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>
                            </div>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="Tab_Empleados" runat="server" HeaderText="Empleados">
                        <HeaderTemplate>Empleados</HeaderTemplate>
                        <ContentTemplate>
                            <table style="width:100%;">
                                <tr>
                                    <td style="text-align:left;width:20%;">U. Responsable</td>
                                    <td style="text-align:left;width:80%;" colspan="3">
                                        <asp:DropDownList ID="Cmb_Dependencia_Domingo_Trabajado" runat="server" 
                                            Width="100%" 
                                            onselectedindexchanged="Cmb_Dependencia_Domingo_Trabajado_SelectedIndexChanged"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:left;width:10%;">*Empleado</td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Empleado_Domingo_Trabajado" runat="server" Width="100%"  style="font-size:12px"
                                            onkeyup='this.value = this.value.toUpperCase();'/>
                                            
                                        <cc1:TextBoxWatermarkExtender ID="TBW_Txt_Empleado_Domingo_Trabajado" runat="server" TargetControlID="Txt_Empleado_Domingo_Trabajado"
                                            WatermarkCssClass="watermarked2" WatermarkText="Nombre ó No. Empleado" Enabled="True"/>
                                    </td>
                                    <td style="text-align:left;width:3%;">
                                        <asp:ImageButton ID="Btn_Buscar_Empleado_Domingo_Trabajado" runat="server" 
                                            Text="Buscar"
                                             ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                            onclick="Btn_Buscar_Empleado_Domingo_Trabajado_Click"/>
                                    </td>
                                    <td style="text-align:left;width:45%;">
                                        <asp:DropDownList ID="Cmb_Empleado_Domingo_Trabajado" runat="server" Width="78%"
                                            style="font-size:11px"/>
                                        <asp:Button ID="Btn_Agregar_Empleado_Domingo_Trabajado" runat="server" 
                                            Text="Agregar" 
                                            Width="20%" Font-Size="11px" Height="22px" 
                                            onclick="Btn_Agregar_Empleado_Domingo_Trabajado_Click"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:100%;" colspan="4">
                                        <div style="overflow:auto;height:400px;width:99%;vertical-align:top;border-style:outset;color:White;" >
                                            <asp:GridView ID="Grid_Empleados_Domingos_Trabajado" runat="server" CssClass="GridView_1"
                                                AutoGenerateColumns="False" GridLines="None" 
                                                onpageindexchanging="Grid_Empleados_Domingos_Trabajado_PageIndexChanging" 
                                                onrowcommand="Grid_Empleados_Domingos_Trabajado_RowCommand">
                                                <Columns>                                                
                                                    <asp:BoundField DataField="EMPLEADO_ID" HeaderText="Empleado ID">
                                                        <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="NO_EMPLEADO" HeaderText="No Empleado">
                                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Empleado" HeaderText="Nombre">
                                                        <HeaderStyle HorizontalAlign="Left" Width="55%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="55%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Eliminar">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="Btn_Eliminar_Empleado" runat="server" CommandName="Eliminar_Empleado" 
                                                                ImageUrl="~/paginas/imagenes/gridview/grid_garbage.png" 
                                                                OnClientClick="return confirm('¿Está seguro de eliminar el Empleado seleccionado?');" 
                                                                CommandArgument='<%# Eval("EMPLEADO_ID") %>'/>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                                        <ItemStyle HorizontalAlign="Center" Width="15%" />                                                        
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
            <asp:AsyncPostBackTrigger ControlID="Btn_Busqueda_Domingos_Trabajados" EventName="Click"/>
        </Triggers>
        
    </asp:UpdatePanel>
    
    <asp:UpdatePanel ID="Pnl_Modal" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
                <<cc1:ModalPopupExtender ID="MPE_Msj" runat="server" BackgroundCssClass="popUpStyle"  BehaviorID="Busqueda_Empleados"
                    PopupControlID="Pnl_Mensaje" TargetControlID="Btn_Axuliar" PopupDragHandleControlID="Pnl_Interno" 
                    CancelControlID="Btn_Comodin" DropShadow="True" DynamicServicePath="" Enabled="True" />  
                <asp:Button Style="background-color: transparent; border-style:none;" ID="Btn_Comodin" runat="server" Text="" />  
                <asp:Button Style="background-color: transparent; border-style:none;" ID="Btn_Axuliar" runat="server" Text="" />   
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Btn_Busqueda_Domingos_Trabajados" EventName="Click"/>
        </Triggers>    
    </asp:UpdatePanel>
    
    
    <asp:Panel ID="Pnl_Mensaje" runat="server" HorizontalAlign="Center" Width="650px" 
         style="display:none;border-style:outset;border-color:Silver;background-image:url(../imagenes/paginas/fondo1.jpg);background-repeat:repeat-y;">                         
    <asp:Panel ID="Pnl_Interno" runat="server" 
        style="cursor: move;background-color:Silver;color:White;font-size:12;font-weight:bold;border-style:outset;">
        <table width="99%">
            <tr>
                <td style="color:Black;font-size:12;font-weight:bold;text-align:left;">
                   <asp:Image ID="Image1" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                     Informacion: B&uacute;squeda de Prima Dominical en el Sistema SIAG
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
            
                <div style="cursor:default;width:100%;background-color:Transparent;">
                    <center>
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
                                    No Domingos
                                </td>
                                <td style="width:30%;text-align:left;font-size:11px;">
                                    <asp:TextBox ID="Txt_Busqueda_No_Domingo" runat="server" Width="98%" MaxLength="10"/>
                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_No_Domingo"
                                            runat="server" TargetControlID="Txt_Busqueda_No_Domingo" FilterType="Numbers"/>
                                    <cc1:TextBoxWatermarkExtender ID="Twm_Busqueda_No_Domingo" runat="server" 
                                        TargetControlID ="Txt_Busqueda_No_Domingo" WatermarkText="No Domingo" 
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
                                    <asp:DropDownList ID="Cmb_Busqueda_Dependencia" runat="server" Width="100%"/>
                                </td>
                            </tr>
                            <tr>
                                <td style="width:20%;text-align:left;font-size:11px;">
                                    Fecha Inicio
                                </td>
                                <td style="width:30%;text-align:left;font-size:11px;">
                                        <asp:TextBox ID="Txt_Busqueda_Fecha_Inicio" runat="server" Width="85%" MaxLength="1" />
                                        <cc1:FilteredTextBoxExtender ID="FTB_Txt_Busqueda_Fecha_Inicio" runat="server" ValidChars="/" 
                                            TargetControlID="Txt_Busqueda_Fecha_Inicio" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters"/>
                                        <cc1:CalendarExtender ID="CE_Txt_Busqueda_Fecha_Inicio" runat="server" OnClientShown="calendarShown"
                                            TargetControlID="Txt_Busqueda_Fecha_Inicio" PopupButtonID="Btn_Busqueda_Fecha_Inicio" Format="dd/MMM/yyyy"/>
                                        <asp:ImageButton ID="Btn_Busqueda_Fecha_Inicio" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                            ToolTip="Seleccione la Fecha"/>
                                </td>
                                <td style="width:20%;text-align:left;font-size:11px;">
                                    Fecha Fin
                                </td>
                                <td style="width:30%;text-align:left;font-size:11px;">
                                        <asp:TextBox ID="Txt_Busqueda_Fecha_Fin" runat="server" Width="85%" MaxLength="1"/>
                                        <cc1:FilteredTextBoxExtender ID="FTB_Txt_Busqueda_Fecha_Fin" ValidChars="/" runat="server"
                                            TargetControlID="Txt_Busqueda_Fecha_Fin" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters"/>
                                        <cc1:CalendarExtender ID="CE_Txt_Busqueda_Fecha_Fin" runat="server" OnClientShown="calendarShown"
                                            TargetControlID="Txt_Busqueda_Fecha_Fin" PopupButtonID="Btn_Busqueda_Fecha_Fin" Format="dd/MMM/yyyy"/>
                                        <asp:ImageButton ID="Btn_Busqueda_Fecha_Fin" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                            ToolTip="Seleccione la Fecha"/>
                                </td>
                            </tr>
                           <tr>
                                <td style="width:100%" colspan="4"><hr /></td>
                            </tr>
                           <tr>
                                <td style="width:100%" colspan="4" align="center">
                                    <asp:Button ID="Btn_Busqueda_Domingos_Trabajados" runat="server" Text="Búsqueda Domingos Trabajados" OnClick="Btn_Busqueda_Domingos_Trabajados_Click" CssClass="button_autorizar"
                                         style="background-image:url(../imagenes/paginas/busqueda.png); background-repeat: no-repeat; background-position:right; cursor:hand;" ToolTip="Ejecutar la búsqueda de Domingos Trabajados en el sistema"/>
                                </td>
                            </tr>
                           <tr>
                                <td style="width:100%" colspan="4"><hr /></td>
                            </tr>
                        </table>
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
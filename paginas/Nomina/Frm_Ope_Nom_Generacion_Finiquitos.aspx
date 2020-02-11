<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Nom_Generacion_Finiquitos.aspx.cs" Inherits="paginas_Nomina_Frm_Ope_Nom_Generacion_Finiquitos" Title="Generación de Finiquitos" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
    <script src="../../easyui/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script src="../../easyui/jquery.formatCurrency-1.4.0.min.js" type="text/javascript"></script>
    <script src="../../easyui/jquery.formatCurrency.all.js" type="text/javascript"></script>

    <script src="../../javascript/Js_Generacion_Finiquitos.js" type="text/javascript"></script>

        <script type="text/javascript">
            function Chk_Select_All_Percepciones(CheckBox) {
                var Concepto_ID = "";
                var Chk_Conceptos_Exclusivos_Finiquito = '#<%=Grid_Conceptos_Exclusivos_Finiquitos.ClientID%> input[id*="Chk_Aplica_Percepcion_Finiquito"]:checkbox';
                
                var Percepcion_ID = "";
                var CheckBox_Interno = '#<%=Grid_Percepciones.ClientID%> input[id*="Chk_Aplica_Deduccion_Finiquito"]:checkbox';
                $(CheckBox_Interno).attr('checked', CheckBox.checked);
                
                $(CheckBox_Interno).each(function (){
                    Percepcion_ID  = $(this).parent().attr('class');
                    $(Chk_Conceptos_Exclusivos_Finiquito).each(function(){
                        Concepto_ID = $(this).parent().attr('class');
                        if(Percepcion_ID == Concepto_ID){
                            $(this).attr('checked', CheckBox.checked);
                        }
                    });
                });
             }
             
            function Seleccionar_Todas_Deducciones(CheckBox) {
                var Concepto_ID ="";
                var Chk_Conceptos_Exclusivos_Finiquito = '#<%=Grid_Conceptos_Exclusivos_Finiquitos.ClientID%> input[id*="Chk_Aplica_Percepcion_Finiquito"]:checkbox';
            
                var Deduccion_ID = "";
                var CheckBox_Interno = '#<%=Grid_Deducciones.ClientID%> input[id*="Chk_Aplica_Percepcion_Finiquito"]:checkbox';
                $(CheckBox_Interno).attr('checked', CheckBox.checked);
                
                $(CheckBox_Interno).each(function (){
                    Deduccion_ID  = $(this).parent().attr('class');
                    $(Chk_Conceptos_Exclusivos_Finiquito).each(function(){
                        Concepto_ID = $(this).parent().attr('class');
                        if(Deduccion_ID == Concepto_ID){
                            $(this).attr('checked', CheckBox.checked);
                        }
                    });
                });
             }
             
             function Habilitar(CHECKBOX){
                var Percepcion_ID = "";
                var Deduccion_ID = "";
                var Percepcion_Deduccion_ID = $(CHECKBOX).parent().attr('class');
                var CheckBox_Percepciones_Interno = '#<%=Grid_Percepciones.ClientID%> input[id*="Chk_Aplica_Deduccion_Finiquito"]:checkbox';
                var CheckBox_Deducciones_Interno = '#<%=Grid_Deducciones.ClientID%> input[id*="Chk_Aplica_Percepcion_Finiquito"]:checkbox';
                
                $(CheckBox_Percepciones_Interno).each(function(){
                    Percepcion_ID = $(this).parent().attr('class');
                    if(Percepcion_Deduccion_ID == Percepcion_ID){
                        $(this).attr('checked', CHECKBOX.checked);
                    }
                });
                
                $(CheckBox_Deducciones_Interno).each(function(){
                    Deduccion_ID = $(this).parent().attr('class');
                    if(Percepcion_Deduccion_ID == Deduccion_ID){
                        $(this).attr('checked', CHECKBOX.checked);
                    }
                });
             }
             
        function pageLoad(sender, args) {
            $('input[id$=Txt_No_Empleado]').bind("blur", function(){
                var Ceros = "";
                if($(this).val() != undefined){
                    for(i=0;    i<(6-$(this).val().length);    i++){
                        Ceros += '0';
                    }
                    $(this).val(Ceros + $(this).val());
                    Ceros = "";
                }
            });
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
    <cc1:ToolkitScriptManager ID="Tsm_Generar_Nomina" runat="server"  AsyncPostBackTimeout="36000"/>
    <asp:UpdatePanel ID="UPnl_Generacion_Finiquitos" runat="server">
        <ContentTemplate>
        
            <asp:UpdateProgress ID="UProgess_Generacion_Finiquitos" runat="server" AssociatedUpdatePanelID="UPnl_Generacion_Finiquitos" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                     <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            
            <div id="Contenedor_Principal"  style="background-color:#ffffff; width:99%; height:100%;">
            
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr>
                        <td>
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false"  />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"/>
                        </td>
                    </tr>
                </table>
            
                <table style="width:100%;">
                    <tr>
                        <td style="width:100%;" align="center">
                            <div id="Contenedor_Titulo" style="background-color:Silver;color:White;font-size:12;font-weight:bold;border-style:outset;">
                                <table width="100%">
                                    <tr>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td width="100%">
                                            <font style="color: Black; font-weight: bold;">Generaci&oacute;n de Finiquitos</font>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
                
                 <asp:Panel ID="Pnl_Detalles_Nomina" runat="server" GroupingText="Nomina">
                    <table style="width:100%;">
                        <tr>
                            <td style="width:100%;" colspan="4" align="center">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td style="width:20%;font-size:12px;">
                                <asp:Label ID="Lbl_Tipo_Nomina" runat="server" Text="Tipo Nómina" Width="100%"/>
                            </td>
                            <td style="width:30%;font-size:12px;">
                                <asp:DropDownList ID="Cmb_Tipo_Nomina" runat="server" Width="100%" Enabled="false"/>
                            </td>
                            <td style="width:20%;font-size:12px;">
                                
                            </td>
                            <td style="width:30%;font-size:12px;">
                                <asp:Button ID="Btn_Cerrar_Finiquito" runat="server" Text="Cerrar Finiquito" CausesValidation="false"
                                    onclick="Btn_Cerrar_Finiquito_Click" CssClass="button_autorizar" ToolTip="Cerrar Finiquito"/>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:20%;font-size:12px;">
                                <asp:Label ID="Lbl_Calendario_Nomina" runat="server" Text="Nomina" Width="100%"/>
                            </td>
                            <td style="width:30%;font-size:12px;">
                                <asp:DropDownList ID="Cmb_Calendario_Nomina" runat="server" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="Cmb_Calendario_Nomina_SelectedIndexChanged"/>
                            </td>
                            <td style="width:20%;font-size:12px;">
                                <asp:Label ID="Lbl_Periodo" runat="server" Text="Periodo" Width="100%" />
                            </td>
                            <td style="width:30%;font-size:12px;">
                                <asp:DropDownList ID="Cmb_Periodo" runat="server" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="Cmb_Periodo_SelectedIndexChanged"/>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:20%;font-size:12px;">
                                <asp:Label ID="Lbl_Inicia_Catorcena" runat="server" Text="Inicia Catorcena" Width="100%"/>
                            </td>
                            <td style="width:30%;font-size:12px;">
                                <asp:TextBox ID="Txt_Inicia_Catorcena" runat="server" Width="98%" Enabled="false"/>
                            </td>
                            <td style="width:20%;font-size:12px;">
                                <asp:Label ID="Lbl_Fin_Catorcena" runat="server" Text="Fin Catorcena" Width="100%"/>
                            </td>
                            <td style="width:30%;font-size:12px;">
                                <asp:TextBox ID="Txt_Fin_Catorcena" runat="server" Width="98%" Enabled="false"/>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:100%;" colspan="4" align="center">
                                <hr />
                            </td>
                        </tr>
                    </table>
                 </asp:Panel>
                
                <asp:Panel ID="Pnl_Buscar_Empleado" runat="server" GroupingText="Empleado">
                    <table width="98%">
                        <tr>
                            <td style="width:20%;text-align:left">
                                *No Empleado
                            </td>  
                            <td style="width:30%;text-align:left;">
                                <asp:TextBox ID="Txt_No_Empleado" runat="server" Width="98%" TabIndex="0" 
                                    onblur="$('.face').text(''); $('select[id$=Cmb_Tipo_Salario]').get(0).selectedIndex = 0"/>
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_No_Empleado" runat="server" FilterType="Numbers"
                                    TargetControlID="Txt_No_Empleado"/> 
                                <cc1:TextBoxWatermarkExtender ID="Twm_Txt_No_Empleado" runat="server" 
                                    TargetControlID ="Txt_No_Empleado" 
                                    WatermarkText="No Empleado" WatermarkCssClass="watermarked"/>
                            </td>
                            <td style="width:20%;text-align:left;">
                                Clase Nomina
                            </td>
                            <td style="width:30%;text-align:left;">
                                <asp:TextBox ID="Txt_Clase_Nomina_Empleado" runat="server" Width="123px" Height="33px" Enabled="false"
                                    style="font-family:Lucida Grande; font-size:13px; font-weight:bold; border-style:none;
                                    cursor:default;width:123px;height:20px;background-color:White;text-align:center;
                                    background-image:url(../imagenes/paginas/Sombra.png); background-repeat:no-repeat;
                                    vertical-align:middle;padding-top:12.5px;border-bottom:ridge 1px Silver;"/>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:ImageButton ID="Btn_Buscar_Empleado" runat="server"  Width="22px" Height="22px"
                                    TabIndex="1" ImageUrl="~/paginas/imagenes/paginas/icono_consultar.png" 
                                    onclick="Btn_Buscar_Empleado_Click" CausesValidation="false"
                                    ToolTip="Buscar Empleado"/>
                                <asp:ImageButton ID="IBtn_Refrescar_Pantalla" runat="server" Width="22px" Height="22px"
                                    TabIndex="1" ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png" 
                                    onclick="IBtn_Refrescar_Pantalla_Click" CausesValidation="false"
                                    ToolTip="Limpiar Campos."/>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:20%;text-align:left;">
                                Nombre Empleado
                            </td>
                            <td style="width:80%;text-align:left;" colspan="3">
                                <asp:TextBox ID="Txt_Nombre_Empleado" runat="server" Width="99.5%" Enabled="false"/>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:20%;font-size:12px;vertical-align:top;">
                                Tipo Salario
                            </td>
                            <td style="width:30%;font-size:12px;vertical-align:top;">
                                <asp:DropDownList ID="Cmb_Tipo_Salario" runat="server" Width="100%" AutoPostBack="true"
                                    OnSelectedIndexChanged="Cmb_Tipo_Salario_SelectedIndexChanged">
                                    <asp:ListItem>&lt; - Seleccione -- &gt;</asp:ListItem>
                                    <asp:ListItem>Salario Diario</asp:ListItem>
                                    <asp:ListItem>SMG2</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width:20%;font-size:12px;">
                                
                            </td>
                            <td style="width:30%;font-size:12px;" align="right">
                                <asp:Label ID="Lbl_Cantidad_Salario" runat="server" Text="" Width="100%"
                                    style="font-family:Lucida Grande; font-size:16px; font-weight:bold; border-style:none;
                                    cursor:default;width:155px;height:26px;background-color:White;text-align:center;
                                    background-image:url(../imagenes/paginas/icono_rechazado.png); background-repeat:no-repeat;
                                    vertical-align:middle;padding-top:3.5px;" class="face"/>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            
                <asp:Panel ID="Pnl_Datos_Empleado" runat="server" GroupingText="Datos Empleado">
                    <table width="100%">
                        <tr>
                            <td style="width:15%;vertical-align:top ;" align="center">
                                <asp:Image ID="Img_Foto_Empleado" runat="server"  Width="100px" Height="115px" BorderWidth="1px"
                                    ImageUrl="~/paginas/imagenes/paginas/Sias_No_Disponible.JPG" BorderColor="Silver" BorderStyle="Outset"/>
                            </td>
                            <td style="width:85%;">
                                <table width="100%">
                                    <tr>
                                        <td style="width:100%;" colspan="4">
                                            <table width="100%">
                                                <tr>
                                                    <td style="width:20%;text-align:left;">
                                                        Fecha Elaboración
                                                    </td>
                                                    <td style="width:20%;text-align:left;">
                                                        <asp:TextBox ID="Txt_Fecha_Elaboracion" runat="server" Width="98%" ReadOnly="true"
                                                                style="font-family:Lucida Grande; font-size:12px; font-weight:bold; border-style:none;
                                                                cursor:default;width:99.5%;height:20px;background-color:White;text-align:center;
                                                                background-image:url(../imagenes/paginas/Fecha_Critica.png); background-repeat:no-repeat;
                                                                vertical-align:middle;padding-top:12.5px"/>
                                                    </td>
                                                    <td style="width:20%;text-align:left;">
                                                        
                                                    </td>
                                                    <td style="width:40%;text-align:left;">
                                                        
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;">
                                            Puesto
                                        </td>
                                        <td style="width:80%;text-align:left;" colspan="3">
                                            <asp:TextBox ID="Txt_Puesto" runat="server" Width="99.5%" Enabled="false"
                                                    style="font-family:Lucida Grande; font-size:12px; font-weight:bold; border-width:2px; border-color:#f5f5f5;
                                                    cursor:default;width:99.5%;background-color:#f5f5f5;text-align:left;border-style:ridge;"/>
                                        </td>
                                    </tr> 
                                    <tr>
                                        <td style="width:20%;text-align:left;">
                                            Sindicato
                                        </td>
                                        <td style="width:80%;text-align:left;" colspan="3">
                                            <asp:TextBox ID="Txt_Sindicato_Empleado" runat="server" Width="99.5%" Enabled="false"
                                                    style="font-family:Lucida Grande; font-size:12px; font-weight:bold; border-width:2px; border-color:#f5f5f5;
                                                    cursor:default;width:99.5%;background-color:#f5f5f5;text-align:left;border-style:ridge;"/>
                                        </td>
                                    </tr>   
                                    <tr>
                                        <td style="width:20%;text-align:left;">
                                            U. Responsable
                                        </td>
                                        <td style="width:80%;text-align:left;" colspan="3">
                                            <asp:TextBox ID="Txt_Dependencia_Empelado" runat="server" Width="99.5%" Enabled="false"
                                                    style="font-family:Lucida Grande; font-size:12px; font-weight:bold; border-width:2px; border-color:#f5f5f5;
                                                    cursor:default;width:99.5%;background-color:#f5f5f5;text-align:left;border-style:ridge;"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;">
                                            Concepto de Baja
                                        </td>
                                        <td style="width:80%;text-align:left;" colspan="3">
                                            <asp:TextBox ID="Txt_Concepto_Baja" runat="server" Width="99.5%" Enabled="false"
                                                    style="font-family:Lucida Grande; font-size:12px; font-weight:bold; border-width:2px; border-color:#f5f5f5;
                                                    cursor:default;width:99.5%;background-color:#f5f5f5;text-align:left;border-style:ridge;"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;">
                                            Fecha Ingreso
                                        </td>
                                        <td style="width:30%;text-align:left;">
                                            <asp:TextBox ID="Txt_Fecha_Ingreso" runat="server" Width="98%" Enabled="false"
                                                    style="font-family:Lucida Grande; font-size:12px; font-weight:bold; border-width:2px; border-color:#f5f5f5;
                                                    cursor:default;width:99.5%;background-color:#f5f5f5;text-align:center;border-style:ridge;"/>
                                        </td>
                                        <td style="width:20%;text-align:left;">
                                            &nbsp;&nbsp;Fecha Baja
                                        </td>
                                        <td style="width:30%;text-align:left;">
                                            <asp:TextBox ID="Txt_Fecha_Baja" runat="server" Width="98%" Enabled="false"
                                                    style="font-family:Lucida Grande; font-size:12px; font-weight:bold; border-width:2px; border-color:#f5f5f5;
                                                    cursor:default;width:99.5%;background-color:#f5f5f5;text-align:center;border-style:ridge;"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;">
                                            Salario Diario
                                        </td>
                                        <td style="width:30%;text-align:left;">
                                            <asp:TextBox ID="Txt_Salario_Diario" runat="server" Width="98%" Enabled="false"
                                                style="font-family:Lucida Grande; font-size:12px; font-weight:bold; border-width:2px; border-color:#f5f5f5;
                                                       cursor:default;width:98%;background-color:#f5f5f5;text-align:right;border-style:ridge;"/>
                                        </td>
                                        <td style="width:20%;text-align:left;">
                                            &nbsp;&nbsp;Salario Mensual
                                        </td>
                                        <td style="width:30%;text-align:left;">
                                            <asp:TextBox ID="Txt_Salario_Mensual" runat="server" Width="98%" Enabled="false"
                                                       style="font-family:Lucida Grande; font-size:12px; font-weight:bold; border-width:2px; border-color:#f5f5f5;
                                                       cursor:default;width:98%;background-color:#f5f5f5;text-align:right;border-style:ridge;"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;">
                                            S.D.I
                                        </td>
                                        <td style="width:30%;text-align:left;">
                                            <asp:TextBox ID="Txt_Salario_Diario_Integrado" runat="server" Width="98%" Enabled="false"
                                                style="font-family:Lucida Grande; font-size:12px; font-weight:bold; border-width:2px; border-color:#f5f5f5;
                                                cursor:default;width:98%;background-color:#f5f5f5;text-align:right;border-style:ridge;"/>
                                        </td>   
                                        <td style="width:20%;text-align:left;">
                                            &nbsp;&nbsp;Costo Por Hora
                                        </td>
                                        <td style="width:30%;text-align:left;">
                                            <asp:TextBox ID="Txt_Costo_Por_Hora" runat="server" Width="98%" Enabled="false"
                                                style="font-family:Lucida Grande; font-size:12px; font-weight:bold; border-width:2px; border-color:#f5f5f5;
                                                cursor:default;width:98%;background-color:#f5f5f5;text-align:right;border-style:ridge;"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;">
                                            C&oacute;digo Program&aacute;tico
                                        </td>
                                        <td style="width:80%;text-align:left;" colspan="3">
                                            <asp:TextBox ID="Txt_Codigo_Programatico" runat="server" Enabled="false"
                                                    style="font-family:Lucida Grande; font-size:12px; font-weight:bold; border-width:2px; border-color:#f5f5f5;
                                                    cursor:default;width:99.5%;background-color:#f5f5f5;text-align:center;border-style:ridge;"/>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>  
                 </asp:Panel>
                 
                 <asp:Panel ID="Pnl_Operaciones" runat="server">
                    <table width="100%">
                        <tr>
                            <td style="width:100%;" colspan="4" align="right">
                                <table style="width:100%;">
                                    <tr>
                                        <td style="width:30%;">
                                            <asp:Button ID="Btn_Actualizar_Tablas_Conceptos" runat="server" Text="Actualizar" CausesValidation="false"
                                                OnClick="Btn_Actualizar_Tablas_Conceptos_Click"  CssClass="button_autorizar" />
                                        </td>
                                        <td style="width:40%;">
                                            <asp:Button ID="Btn_Generar_Finiquito" runat="server" Text="Generar Finiquito" CausesValidation="false"
                                                onclick="Btn_Generar_Finiquito_Click" CssClass="button_autorizar" ToolTip="Generar Finiquito"/>
                                        </td>
                                        <td style="width:30%;">
                                            <input type="button" id="Btn_Ver_Detalles_Finiquito" value="Ocultar Detalles Nomina"
                                                class="button_autorizar" onclick="if(this.value == 'Ocultar Detalles Nomina')this.value='Ver Detalles Nomina';else this.value='Ocultar Detalles Nomina';"/>                                                                                                     
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                
                <div id="Contenedor" style="overflow:auto;height:450px;width:100%;vertical-align:top;border-style:none;border-color:Silver;" >
                <cc1:TabContainer ID="TC_Percepciones_Deducciones_Resguardos" runat="server" Width="98%" ActiveTabIndex="0">
                    <cc1:TabPanel ID="TPnl_Percepciones" runat="server" HeaderText="Percepciones">
                        <HeaderTemplate>Percepciones</HeaderTemplate>
                        <ContentTemplate>
                            <table style="width:99%;">
                                <tr>
                                    <td style="width:100%;" colspan="4" align="center">
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:100%;" align="center">
                                        <div id="Div1" style="background-color:Silver;color:White;font-size:12;font-weight:bold;border-style:outset;">
                                            <font style="color:Black;font-weight:bold;">Percepciones Aplican  Finiquito</font>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:100%;" colspan="4" align="center" >
                                        <hr />
                                    </td>
                                </tr>
                            </table>
                            <table style="width:100%;">
                                <tr>
                                    <td style="width:100%;text-align:center;vertical-align:top;" colspan="3">
                                        <center>
                                        <div >
                                            <table style="width:100%;" border="0">
                                                <th style="width:2%;" class="GridHeader_Nested">
                                                    <input id="Chk_Seleccionar_Todos" type="checkbox" checked="checked"
                                                        onclick="javascript:Chk_Select_All_Percepciones(this);"/>
                                                </th>
                                                <th style="width:35%;" class="GridHeader_Nested">Nombre</th>
                                                <th style="width:15%;" class="GridHeader_Nested">Tipo</th>
                                                <th style="width:20%;" class="GridHeader_Nested">Cantidad</th>
                                                <th style="width:20%;" class="GridHeader_Nested">Cantidad Real</th>
                                            </table>
                                        </div>
                                        <div>
                                        <asp:GridView ID="Grid_Percepciones" runat="server" ShowHeader="false"
                                                    AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None"
                                                    Width="100%" OnRowDataBound="Grid_Percepciones_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="Chk_Aplica_Deduccion_Finiquito" runat="server" Checked="true"
                                                                    CssClass='<%# Eval("PERCEPCION_DEDUCCION_ID") %>'/>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                       <asp:BoundField DataField="PERCEPCION_DEDUCCION_ID" HeaderText="Clave">
                                                            <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="0%" Font-Size="11px" Font-Bold="true"/>
                                                       </asp:BoundField>
                                                       <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" >
                                                            <HeaderStyle HorizontalAlign="Left" Width="45%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="45%" Font-Size="11px" Font-Bold="true"/>
                                                       </asp:BoundField>
                                                       <asp:BoundField DataField="TIPO_ASIGNACION" HeaderText="Tipo" >
                                                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="15%" Font-Size="10px" Font-Bold="true"/>
                                                       </asp:BoundField>
                                                       <asp:TemplateField HeaderText="Cantidad">
                                                            <ItemTemplate>
                                                                  <asp:TextBox ID="Txt_Cantidad_Percepcion" runat="server" class="Campos_Cantidad_Normal" 
                                                                    onblur="$('input[id$=Txt_Cantidad_Percepcion]').formatCurrency({colorize:true, region: 'es-MX'});$('input[id$=Txt_Cantidad_Percepcion]').removeClass('Campos_Cantidad'); $('input[id$=Txt_Cantidad_Percepcion]').addClass('Campos_Cantidad_Normal');" 
                                                                    onclick="$('input[id$=Txt_Cantidad_Percepcion]').removeClass('Campos_Cantidad_Normal');$('input[id$=Txt_Cantidad_Percepcion]').addClass('Campos_Cantidad');" 
                                                                    Text='<%# Eval("Cantidad") %>'/>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="20%"/>
                                                       </asp:TemplateField> 
                                                       <asp:TemplateField HeaderText="Cantidad">
                                                            <ItemTemplate>
                                                                  <asp:TextBox ID="Txt_Cantidad_Percepcion_Real" runat="server" CssClass="Campos_Cantidad_Normal"
                                                                    onblur="$('input[id$=Txt_Cantidad_Percepcion_Real]').formatCurrency({colorize:true, region: 'es-MX'});$('input[id$=Txt_Cantidad_Percepcion_Real]').removeClass('Campos_Cantidad');$('input[id$=Txt_Cantidad_Percepcion_Real]').addClass('Campos_Cantidad_Normal');"
                                                                    onclick="$('input[id$=Txt_Cantidad_Percepcion_Real]').removeClass('Campos_Cantidad_Normal');$('input[id$=Txt_Cantidad_Percepcion_Real]').addClass('Campos_Cantidad');" 
                                                                    Text='<%# Eval("Cantidad") %>'/>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="20%"/>
                                                       </asp:TemplateField> 
                                                       <asp:BoundField DataField="Gravado" HeaderText="Gravado" />
                                                       <asp:BoundField DataField="Exento" HeaderText="Exento" />
                                                    </Columns>
                                                   <SelectedRowStyle CssClass="GridSelected" />
                                                   <PagerStyle CssClass="GridHeader" />
                                                   <HeaderStyle CssClass="GridHeader" />
                                                   <AlternatingRowStyle CssClass="GridAltItem" />
                                        </asp:GridView>
                                        </div>
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="TPnl_Deducciones" runat="server" HeaderText="Deducciones">
                        <HeaderTemplate>Deducciones</HeaderTemplate>
                        <ContentTemplate>
                            <table style="width:99%;">
                                <tr>
                                    <td style="width:100%;" colspan="4" align="center">
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:100%;" align="center">
                                        <div id="Div2" style="background-color:Silver;color:White;font-size:12;font-weight:bold;border-style:outset;">
                                            <font style="color:Black;font-weight:bold;">Deducciones Aplican  Finiquito</font>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:100%;" colspan="4" align="center">
                                        <hr />
                                    </td>
                                </tr>
                            </table>
                            <table style="width:100%;">
                                <tr>
                                    <td style="width:100%;text-align:center;vertical-align:top;" colspan="3">
                                        <center>
                                        <div>
                                            <table style="width:100%;" border="0">
                                                <th style="width:2%;" class="GridHeader_Nested">
                                                    <input id="Chk_Select_All_Deducciones" type="checkbox" checked="checked"
                                                        onclick="javascript:Seleccionar_Todas_Deducciones(this);" />
                                                </th>
                                                <th style="width:35%;" class="GridHeader_Nested">Nombre</th>
                                                <th style="width:15%;" class="GridHeader_Nested">Tipo</th>
                                                <th style="width:20%;" class="GridHeader_Nested">Cantidad</th>
                                                <th style="width:20%;" class="GridHeader_Nested">Cantidad Real</th>
                                            </table>
                                        </div>
                                        <div>
                                        <asp:GridView ID="Grid_Deducciones" runat="server" ShowHeader="false"
                                                    AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None"
                                                    Width="100%" OnRowDataBound="Grid_Deducciones_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="Chk_Aplica_Percepcion_Finiquito" runat="server" Checked="true"
                                                                    CssClass='<%# Eval("PERCEPCION_DEDUCCION_ID") %>'/>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                       <asp:BoundField DataField="PERCEPCION_DEDUCCION_ID" HeaderText="Percepción">
                                                            <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="0%" Font-Size="11px" Font-Bold="true"/>
                                                       </asp:BoundField>
                                                       <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" >
                                                            <HeaderStyle HorizontalAlign="Left" Width="45%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="45%" Font-Size="11px" Font-Bold="true"/>
                                                       </asp:BoundField> 
                                                       <asp:BoundField DataField="TIPO_ASIGNACION" HeaderText="Tipo" >
                                                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="15%" Font-Size="10px" Font-Bold="true"/>
                                                       </asp:BoundField>
                                                       <asp:TemplateField HeaderText="Cantidad">
                                                            <ItemTemplate>
                                                                  <asp:TextBox ID="Txt_Cantidad_Deduccion" runat="server" CssClass="Campos_Cantidad_Normal"
                                                                    onblur="$('input[id$=Txt_Cantidad_Deduccion]').formatCurrency({colorize:true, region: 'es-MX'});$('input[id$=Txt_Cantidad_Deduccion]').removeClass('Campos_Cantidad'); $('input[id$=Txt_Cantidad_Deduccion]').addClass('Campos_Cantidad_Normal');" 
                                                                    onclick="$('input[id$=Txt_Cantidad_Deduccion]').removeClass('Campos_Cantidad_Normal');$('input[id$=Txt_Cantidad_Deduccion]').addClass('Campos_Cantidad');" 
                                                                    />
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="20%"/>
                                                       </asp:TemplateField> 
                                                       <asp:TemplateField HeaderText="Cantidad">
                                                            <ItemTemplate>
                                                                  <asp:TextBox ID="Txt_Cantidad_Deduccion_Real" runat="server" CssClass="Campos_Cantidad_Normal"
                                                                    onblur="$('input[id$=Txt_Cantidad_Deduccion_Real]').formatCurrency({colorize:true, region: 'es-MX'});$('input[id$=Txt_Cantidad_Deduccion_Real]').removeClass('Campos_Cantidad');$('input[id$=Txt_Cantidad_Deduccion_Real]').addClass('Campos_Cantidad_Normal');"
                                                                    onclick="$('input[id$=Txt_Cantidad_Deduccion_Real]').removeClass('Campos_Cantidad_Normal');$('input[id$=Txt_Cantidad_Deduccion_Real]').addClass('Campos_Cantidad');" 
                                                                    />
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="20%"/>
                                                       </asp:TemplateField>
                                                    </Columns>
                                                   <SelectedRowStyle CssClass="GridSelected" />
                                                   <PagerStyle CssClass="GridHeader" />
                                                   <HeaderStyle CssClass="GridHeader" />
                                                   <AlternatingRowStyle CssClass="GridAltItem" />
                                        </asp:GridView>
                                        </div>
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </cc1:TabPanel> 
                    <cc1:TabPanel ID="TPnl_Conceptos_Finiquitos" runat="server" HeaderText="Finiquito">
                        <HeaderTemplate>Finiquito</HeaderTemplate>
                        <ContentTemplate>
                            <table style="width:99%;">
                                <tr>
                                    <td style="width:100%;" colspan="4" align="center">
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:100%;" align="center">
                                        <div id="Div4" style="background-color:Silver;color:White;font-size:12;font-weight:bold;border-style:outset;">
                                            <font style="color:Black;font-weight:bold;">Deducciones Exclusivas Finiquito</font>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:100%;" colspan="4" align="center">
                                        <hr />
                                    </td>
                                </tr>
                            </table>
                            <table style="width:100%;">
                                <tr>
                                    <td style="width:100%;text-align:center;vertical-align:top;" colspan="3">
                                        <center>
                                        <div>
                                            <table style="width:100%;" border="0">
                                                <th style="width:2%;" class="GridHeader_Nested"></th>
                                                <th style="width:35%;" class="GridHeader_Nested">Nombre</th>
                                                <th style="width:15%;" class="GridHeader_Nested">Tipo</th>
                                                <th style="width:20%;" class="GridHeader_Nested">Cantidad</th>
                                                <th style="width:20%;" class="GridHeader_Nested">Cantidad Real</th>
                                            </table>
                                        </div>
                                        <div >
                                        <asp:GridView ID="Grid_Conceptos_Exclusivos_Finiquitos" runat="server" ShowHeader="false"
                                                    AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None"
                                                    Width="100%" OnRowDataBound="Grid_Conceptos_Exclusivos_Finiquitos_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="Chk_Aplica_Percepcion_Finiquito" runat="server" Checked="true" CssClass='<%# Eval("PERCEPCION_DEDUCCION_ID") %>'
                                                                    onclick="javascript:Habilitar(this);"/>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                       <asp:BoundField DataField="PERCEPCION_DEDUCCION_ID" HeaderText="Percepción">
                                                            <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="0%" Font-Size="11px" Font-Bold="true"/>
                                                       </asp:BoundField>
                                                       <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" >
                                                            <HeaderStyle HorizontalAlign="Left" Width="45%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="45%" Font-Size="11px" Font-Bold="true"/>
                                                       </asp:BoundField>
                                                       <asp:BoundField DataField="TIPO_ASIGNACION" HeaderText="Tipo" >
                                                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="15%" Font-Size="10px" Font-Bold="true"/>
                                                       </asp:BoundField>
                                                       <asp:TemplateField HeaderText="Cantidad">
                                                            <ItemTemplate>
                                                                  <asp:TextBox ID="Txt_Cantidad_Deduccion_Percepcion" runat="server" CssClass="Campos_Cantidad_Normal"
                                                                    onblur="$('input[id$=Txt_Cantidad_Deduccion_Percepcion]').formatCurrency({colorize:true, region: 'es-MX'});$('input[id$=Txt_Cantidad_Deduccion_Percepcion]').removeClass('Campos_Cantidad'); $('input[id$=Txt_Cantidad_Deduccion_Percepcion]').addClass('Campos_Cantidad_Normal');" 
                                                                    onclick="$('input[id$=Txt_Cantidad_Deduccion_Percepcion]').removeClass('Campos_Cantidad_Normal');$('input[id$=Txt_Cantidad_Deduccion_Percepcion]').addClass('Campos_Cantidad');" 
                                                                    />
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="20%"/>
                                                       </asp:TemplateField> 
                                                       <asp:TemplateField HeaderText="Cantidad">
                                                            <ItemTemplate>
                                                                  <asp:TextBox ID="Txt_Cantidad_Deduccion_Percepcion_Real" runat="server" CssClass="Campos_Cantidad_Normal"
                                                                    onblur="$('input[id$=Txt_Cantidad_Deduccion_Percepcion_Real]').formatCurrency({colorize:true, region: 'es-MX'});$('input[id$=Txt_Cantidad_Deduccion_Percepcion_Real]').removeClass('Campos_Cantidad');$('input[id$=Txt_Cantidad_Deduccion_Percepcion_Real]').addClass('Campos_Cantidad_Normal');"
                                                                    onclick="$('input[id$=Txt_Cantidad_Deduccion_Percepcion_Real]').removeClass('Campos_Cantidad_Normal');$('input[id$=Txt_Cantidad_Deduccion_Percepcion_Real]').addClass('Campos_Cantidad');" 
                                                                    />
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="20%"/>
                                                       </asp:TemplateField>
                                                       <asp:BoundField DataField="Gravado" HeaderText="Gravado" />
                                                       <asp:BoundField DataField="Exento" HeaderText="Exento" />
                                                    </Columns>
                                                   <SelectedRowStyle CssClass="GridSelected" />
                                                   <PagerStyle CssClass="GridHeader" />
                                                   <HeaderStyle CssClass="GridHeader" />
                                                   <AlternatingRowStyle CssClass="GridAltItem" />
                                        </asp:GridView>
                                        </div>
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </cc1:TabPanel> 
                    <cc1:TabPanel ID="TPnl_Resguardos_Empleado" runat="server" HeaderText="Resguardos">
                        <HeaderTemplate>Resguardos</HeaderTemplate>
                        <ContentTemplate>
                            <table style="width:99%;">
                                <tr>
                                    <td style="width:100%;" colspan="4" align="center">
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:100%;" align="center">
                                        <div id="Div3" style="background-color:Silver;color:White;font-size:12;font-weight:bold;border-style:outset;">
                                            <font style="color:Black;font-weight:bold;">Resguardos Empleado</font>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:100%;" colspan="4" align="center">
                                        <hr />
                                    </td>
                                </tr>
                            </table>
                            <asp:GridView ID="Grid_Resguardos_Empleado" runat="server" CssClass="GridView_1"
                                AutoGenerateColumns="False" AllowPaging="True" PageSize="7" Width="100%" 
                                GridLines= "None" OnPageIndexChanging="Grid_Resguardos_Empleado_PageIndexChanging">
                                <Columns>
                                    <asp:BoundField DataField="CLAVE_PRODUCTO" HeaderText="Clave">
                                        <HeaderStyle HorizontalAlign="Left" Width="0%"/>
                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NOMBRE_PRODUCTO" HeaderText="Nombre">
                                        <HeaderStyle HorizontalAlign="Left" Width="40%" />
                                        <ItemStyle HorizontalAlign="Left" Width="40%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                    </asp:BoundField> 
                                    <asp:BoundField DataField="TIPO" HeaderText="Tipo">
                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                    </asp:BoundField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Image ID="Img_Reesguardo" runat="server" Width="16px" Height="16px"
                                                ImageUrl='<%# Eval("URL") %>' ToolTip='<%# Eval("TIPO") %>'/>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                        <ItemStyle HorizontalAlign="Center" Width="20%" />
                                    </asp:TemplateField>
                                </Columns>
                                <RowStyle Height="20px" />
                               <SelectedRowStyle CssClass="GridSelected" />
                               <PagerStyle CssClass="GridHeader" />
                               <HeaderStyle CssClass="GridHeader" />
                               <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>
                        </ContentTemplate>
                    </cc1:TabPanel>
             </cc1:TabContainer>
             </div>
             <table style="width:98%;">
                <tr>
                    <td style="width:100%;" colspan="5" align="center">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="width:20%;">
                        <asp:Button ID="Btn_Generar_Pre_Recibo_Finiquito" runat="server" Text="FMTO.LIQ." 
                            OnClick="Btn_Generar_Pre_Recibo_Finiquito_Click" CssClass="button_autorizar"/>
                    </td>
                    <td style="width:20%;">
                        <asp:Button ID="Btn_Generar_Recibo" runat="server" Text="RECIBO" 
                            OnClick="Btn_Generar_Recibo_Finiquito_Click" CssClass="button_autorizar"/>
                    </td>
                    <td style="width:20%;">
                        <asp:Button ID="Btn_Generar_Reporte_Liquidacion" runat="server" Text="FORMATO" 
                            OnClick="Btn_Generar_Reporte_Liquidacion_Click" CssClass="button_autorizar"/>
                    </td>
                    <td style="width:20%;">
                      <asp:Button ID="Btn_Generar_Reporte_Renuncia" runat="server" Text="RENUNCIA" 
                            OnClick="Btn_Generar_Reporte_Renuncia_Click" CssClass="button_autorizar"/>
                    </td>
                    <td style="width:20%;">
                      <asp:Button ID="Btn_Generar_Recibo_Renuncia" runat="server" Text="RECIBO RENUNCIA" 
                            OnClick="Btn_Generar_Recibo_Renuncia_Click" CssClass="button_autorizar"/>
                    </td>
                </tr>
                <tr>
                    <td style="width:100%;" colspan="5" align="center">
                        <hr />
                    </td>
                </tr>
             </table>

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

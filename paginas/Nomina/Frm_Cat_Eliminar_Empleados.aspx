<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Eliminar_Empleados.aspx.cs" Inherits="paginas_Nomina_Frm_Cat_Empleados" Title="Baja de Empleados" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
 <script type="text/javascript" language="javascript">
        //Metodo para mantener los calendarios en una capa mas alat.
         function calendarShown(sender, args){
            sender._popupBehavior._element.style.zIndex = 10000005;
        }

        function Abrir_Modal_Popup() {
            $find('Busqueda_Empleados').show();
            return false;
        }

        function pageLoad(sender, args) {
            if ($("input[id$=Chk_Aplica_Licencia]").attr("checked") === true) {
                $('input[id$=Txt_Fecha_Inicio_Licencia]').removeAttr("disabled");
                $('input[id$=Txt_Termino_Licencia]').removeAttr("disabled");

                $('input[id$=Img_Fecha_Inicio_Licencia]').removeAttr("disabled");
                $('input[id$=Img_Termino_Licencia]').removeAttr("disabled");
            } else {
                $('input[id$=Txt_Fecha_Inicio_Licencia]').attr('disabled', 'disabled');
                $('input[id$=Txt_Termino_Licencia]').attr('disabled', 'disabled');

                $('input[id$=Img_Fecha_Inicio_Licencia]').attr('disabled', 'disabled');
                $('input[id$=Img_Termino_Licencia]').attr('disabled', 'disabled');

                $('input[id$=Txt_Fecha_Inicio_Licencia]').val('');
                $('input[id$=Txt_Termino_Licencia]').val('');
            }
        
            $("input[id$=Btn_Eliminar_Mostrar_Popup]").click(function(e) {
                var nombre = $("input[id$=Txt_Nombre_Empleado]").val() + " " + $("input[id$=Txt_Apellido_Paterno_Empleado]").val() + " " + $("input[id$=Txt_Apellido_Materno_Empleado]").val();
                $("textarea[id$=Txt_Motivo_Baja]").val($("textarea[id$=Txt_Comentarios_Empleado]").val());
                $("input[id$=Txt_Baja_Nombre_Empleado]").val(nombre);
                $("img[id$=Img_Foto_Empleado]").attr("src", $("input[id$=Txt_Ruta_Foto]").val());
                $('input[id$=Txt_Baja_No_Empleado]').val($('input[id$=Txt_No_Empleado]').val());
            });

            $("input[id$=Chk_Aplica_Licencia]").click(function() {
                if (this.checked) {
                    $('input[id$=Txt_Fecha_Inicio_Licencia]').removeAttr("disabled");
                    $('input[id$=Txt_Termino_Licencia]').removeAttr("disabled");

                    $('input[id$=Img_Fecha_Inicio_Licencia]').removeAttr("disabled");
                    $('input[id$=Img_Termino_Licencia]').removeAttr("disabled");
                } else {
                    $('input[id$=Txt_Fecha_Inicio_Licencia]').attr('disabled', 'disabled');
                    $('input[id$=Txt_Termino_Licencia]').attr('disabled', 'disabled');

                    $('input[id$=Img_Fecha_Inicio_Licencia]').attr('disabled', 'disabled');
                    $('input[id$=Img_Termino_Licencia]').attr('disabled', 'disabled');

                    $('input[id$=Txt_Fecha_Inicio_Licencia]').val('');
                    $('input[id$=Txt_Termino_Licencia]').val('');
                }
            });
        
            $('textarea[id*=Txt_Comentarios_Empleado]').keyup(function() {var Caracteres =  $(this).val().length; if (Caracteres > 250) {this.value = this.value.substring(0, 250); Caracteres =  $(this).val().length;  $(this).css("background-color", "Yellow");$(this).css("color", "Red");}else{$(this).css("background-color", "White");$(this).css("color", "Black");}$('#Contador_Caracteres_Comentarios').text('Carácteres Ingresados [ ' + Caracteres + ' ]/[ 250 ]');});

            $('input[id*=Txt_Busqueda_No_Empleado]').live("blur", function() {
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
            });
        }


</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
  
    <asp:ScriptManager ID="ScriptManager_Empleados" runat="server" EnableScriptGlobalization = "true" EnableScriptLocalization = "True"/>
    <asp:UpdatePanel ID="Upd_Panel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>  
        
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>          
            
            <div id="Div_Empleados" style="background-color:#ffffff; width:100%; height:100%;font-size:10px;" >
            <div id="tooltip"></div>
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="4" class="label_titulo">
                            Catálogo de Baja de Empleados
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" >&nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
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
                                                <asp:ImageButton ID="Btn_Eliminar_Mostrar_Popup" runat="server" ToolTip="Eliminar" CssClass="Img_Button" TabIndex="3"
                                                    ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png" OnClientClick="javascript:$find('MPE_Baja').show(); return false;"/>
                                                <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" TabIndex="4"
                                                    ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" onclick="Btn_Salir_Click"/>
                                          </td>
                                          <td align="right" style="width:41%;">
                                            <table style="width:100%;height:28px;">
                                                <tr>
                                                    <td style="width:100%;vertical-align:top;" align="right">
                                                        B&uacute;squeda  
                                                        <asp:ImageButton ID="Btn_Mostrar_Popup_Busqueda" runat="server" ToolTip="Busqueda Avanzada" TabIndex="23" 
                                                            ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="24px" Width="24px"
                                                            OnClientClick="javascript:return Abrir_Modal_Popup();" CausesValidation="false" />
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
              
                <asp:Panel ID="Pnl_Datos_Generales" runat="server"  GroupingText="Datos Generales" Width="97%">
                    <table style="width:98%;">
                        <tr>
                            <td style="width:10%;vertical-align:top;" align="center">
                                <asp:Image ID="Img_Foto_Empleado" runat="server"  Width="70px" Height="85px"
                                    ImageUrl="~/paginas/imagenes/paginas/Sias_No_Disponible.JPG" BorderWidth="1px"/>
                                <asp:HiddenField ID="Txt_Ruta_Foto" runat="server" />  
                            </td>
                            <td style="width:90%;">
                                <table width="100%" class="estilo_fuente">
                                    <tr>
                                        <td style="width:20%;text-align:left;">
                                            Empleado ID
                                        </td>
                                        <td style="width:30%;text-align:left;font-weight:bold;">
                                            <asp:TextBox ID="Txt_Empleado_ID" runat="server" ReadOnly="True" Width="98%"/>
                                        </td>
                                        <td style="width:20%;text-align:left;font-weight:bold;">
                                            &nbsp;&nbsp;*Fecha Baja IMSS
                                        </td>
                                        <td style="width:30%;text-align:left;">
                                            <asp:TextBox ID="Txt_Fecha_Baja_IMSS" runat="server" Width="85%" onblur='this.value = this.value.toLowerCase();'/>
                                             <cc1:CalendarExtender ID="Ce_Fecha_Baja_IMSS" 
                                                runat="server" TargetControlID="Txt_Fecha_Baja_IMSS" 
                                                Format="dd/MMM/yyyy"  PopupButtonID="Img_Fecha_Baja_IMSS"  />
                                             <asp:ImageButton ID="Img_Fecha_Baja_IMSS" runat="server" Width="10%" 
                                                ImageUrl="../imagenes/paginas/SmallCalendar.gif" style="vertical-align:top;"
                                                Height="18px" CausesValidation="false"/> 
                                            <cc1:MaskedEditExtender 
                                                ID="Mee_Fecha_Baja_IMSS" 
                                                Mask="99/LLL/9999" 
                                                runat="server"
                                                MaskType="None" 
                                                UserDateFormat="DayMonthYear" 
                                                UserTimeFormat="None" Filtered="/"
                                                TargetControlID="Txt_Fecha_Baja_IMSS" 
                                                Enabled="True" 
                                                ClearMaskOnLostFocus="false"/>  
                                            <cc1:MaskedEditValidator 
                                                ID="MaskedEditValidator2" 
                                                runat="server" 
                                                ControlToValidate="Txt_Fecha_Baja_IMSS"
                                                ControlExtender="Mee_Fecha_Baja_IMSS" 
                                                EmptyValueMessage=""
                                                InvalidValueMessage="Fecha Baja IMSS Invalida" 
                                                IsValidEmpty="true" 
                                                TooltipMessage="Ingrese o Seleccione la Fecha Baja IMSS"
                                                Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;">
                                            No Empleado
                                        </td>
                                        <td style="width:30%;text-align:left;">
                                            <asp:TextBox ID="Txt_No_Empleado" runat="server" Width="98%" TabIndex="7" MaxLength="20"/> 
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_No_Empleadoo" runat="server"
                                                TargetControlID="Txt_No_Empleado" FilterType="Numbers"/>
                                        </td>
                                        <td style="width:20%;text-align:left;">
                                            &nbsp;&nbsp;Estatus
                                        </td>
                                        <td style="width:30%;text-align:left;">
                                            <asp:DropDownList ID="Cmb_Estatus_Empleado" runat="server" Width="101%" TabIndex="8">
                                                <asp:ListItem>&lt; - Seleccione - &gt;</asp:ListItem>
                                                <asp:ListItem>ACTIVO</asp:ListItem>
                                                <asp:ListItem>INACTIVO</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;">
                                            Nombre
                                        </td>
                                        <td style="width:30%;text-align:left;">
                                            <asp:TextBox ID="Txt_Nombre_Empleado" runat="server" TabIndex="9" MaxLength="100" Width="98%" onkeyup='this.value = this.value.toUpperCase();'/>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Nombre_Empleado" runat="server" TargetControlID="Txt_Nombre_Empleado" 
                                                FilterType="Custom, UppercaseLetters, LowercaseLetters" ValidChars="ÑñáéíóúÁÉÍÓÚ "/>
                                        </td>
                                        <td style="width:20%;text-align:left;font-weight:bold;">
                                            &nbsp;&nbsp;*Fecha Baja
                                        </td>
                                        <td style="width:30%;text-align:left;">
                                            <asp:TextBox ID="Txt_Fecha_Baja" runat="server" Width="85%" onblur='this.value = this.value.toLowerCase();'/>
                                             <cc1:CalendarExtender ID="Ce_Txt_Fecha_Baja" 
                                                runat="server" TargetControlID="Txt_Fecha_Baja" 
                                                Format="dd/MMM/yyyy"  PopupButtonID="Btn_Fecha_Baja"  />
                                             <asp:ImageButton ID="Btn_Fecha_Baja" runat="server" Width="10%" 
                                                ImageUrl="../imagenes/paginas/SmallCalendar.gif" style="vertical-align:top;"
                                                Height="18px" CausesValidation="false"/> 
                                            <cc1:MaskedEditExtender 
                                                ID="Mee_Txt_Fecha_Baja" 
                                                Mask="99/LLL/9999" 
                                                runat="server"
                                                MaskType="None" 
                                                UserDateFormat="DayMonthYear" 
                                                UserTimeFormat="None" Filtered="/"
                                                TargetControlID="Txt_Fecha_Baja" 
                                                Enabled="True" 
                                                ClearMaskOnLostFocus="false"/>  
                                            <cc1:MaskedEditValidator 
                                                ID="Mev_Mee_Txt_Fecha_Baja" 
                                                runat="server" 
                                                ControlToValidate="Txt_Fecha_Baja"
                                                ControlExtender="Mee_Txt_Fecha_Baja" 
                                                EmptyValueMessage=""
                                                InvalidValueMessage="Fecha Baja Invalida" 
                                                IsValidEmpty="true" 
                                                TooltipMessage="Ingrese o Seleccione la Fecha Baja"
                                                Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;">
                                            Apellido Paterno
                                        </td>
                                        <td style="width:30%;text-align:left;">
                                            <asp:TextBox ID="Txt_Apellido_Paterno_Empleado" runat="server" TabIndex="10" MaxLength="100" Width="98%" onkeyup='this.value = this.value.toUpperCase();'/>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Apellido_Paterno_Empleado" runat="server" 
                                                TargetControlID="Txt_Apellido_Paterno_Empleado" FilterType="Custom, UppercaseLetters, LowercaseLetters" 
                                                ValidChars="ÑñáéíóúÁÉÍÓÚ "/>
                                        </td>
                                        <td style="width:20%;text-align:left;">
                                            &nbsp;&nbsp;Apellido Materno
                                        </td>
                                        <td style="width:30%;text-align:left;">
                                            <asp:TextBox ID="Txt_Apellido_Materno_Empleado" runat="server" TabIndex="11" MaxLength="100" Width="98%" onkeyup='this.value = this.value.toUpperCase();'/>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Apellido_Materno_Empleado" runat="server" 
                                                TargetControlID="Txt_Apellido_Materno_Empleado" FilterType="Custom, UppercaseLetters, LowercaseLetters" 
                                                ValidChars="ÑñáéíóúÁÉÍÓÚ "/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;">
                                            Puesto
                                        </td>
                                        <td style="width:80%;text-align:left;" colspan="3">
                                            <asp:DropDownList ID="Cmb_Puestos" runat="server" TabIndex="33" Width="100%"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;font-weight:bold;">
                                            *Tipo de Baja
                                        </td>
                                        <td style="width:30%;text-align:left;">
                                            <asp:DropDownList ID="Cmb_Tipo_Baja" runat="server" Width="100%">
                                                <asp:ListItem>&lt;- Seleccione -&gt;</asp:ListItem>
                                                <asp:ListItem>Abandono de Empleo</asp:ListItem>
                                                <asp:ListItem>Convenir al Buen Servicio</asp:ListItem>
                                                <asp:ListItem>Fallecimiento</asp:ListItem>
                                                <asp:ListItem>Invalidez</asp:ListItem>
                                                <asp:ListItem>Jubilacion</asp:ListItem>
                                                <asp:ListItem>Licencia sin Goze de Sueldo</asp:ListItem>
                                                <asp:ListItem>Renuncia Voluntaria</asp:ListItem>
                                                <asp:ListItem>Resicion</asp:ListItem>
                                                <asp:ListItem>Sustitucion Patronal</asp:ListItem>
                                                <asp:ListItem>Termino Administracion</asp:ListItem>
                                                <asp:ListItem>Termino de Comision</asp:ListItem>
                                                <asp:ListItem>Termino de Contrato</asp:ListItem>
                                                <asp:ListItem>Otros</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width:20%;text-align:left;font-weight:bold;">
                                            &nbsp;&nbsp;*Por Licencia
                                        </td>
                                        <td style="width:30%;text-align:left;">
                                            <asp:CheckBox ID="Chk_Aplica_Licencia" runat="server"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;font-weight:bold;">
                                            *Inicio Licencia
                                        </td>
                                        <td style="width:30%;text-align:left;">
                                            <asp:TextBox ID="Txt_Fecha_Inicio_Licencia" runat="server" Width="85%" onblur='this.value = this.value.toLowerCase();'/>
                                             <cc1:CalendarExtender ID="Ce_Inicio_Licencia" 
                                                runat="server" TargetControlID="Txt_Fecha_Inicio_Licencia" 
                                                Format="dd/MMM/yyyy"  PopupButtonID="Img_Fecha_Inicio_Licencia"  />
                                             <asp:ImageButton ID="Img_Fecha_Inicio_Licencia" runat="server" Width="10%" 
                                                ImageUrl="../imagenes/paginas/SmallCalendar.gif" style="vertical-align:top;"
                                                Height="18px" CausesValidation="false"/> 
                                            <cc1:MaskedEditExtender 
                                                ID="Mee_Inicio_Licencia" 
                                                Mask="99/LLL/9999" 
                                                runat="server"
                                                MaskType="None" 
                                                UserDateFormat="DayMonthYear" 
                                                UserTimeFormat="None" Filtered="/"
                                                TargetControlID="Txt_Fecha_Inicio_Licencia" 
                                                Enabled="True" 
                                                ClearMaskOnLostFocus="false"/>  
                                            <cc1:MaskedEditValidator 
                                                ID="Mev_Iniciio_Licencia" 
                                                runat="server" 
                                                ControlToValidate="Txt_Fecha_Inicio_Licencia"
                                                ControlExtender="Mee_Inicio_Licencia" 
                                                EmptyValueMessage=""
                                                InvalidValueMessage="Fecha Inicio Invalida" 
                                                IsValidEmpty="true" 
                                                TooltipMessage="Ingrese o Seleccione la Fecha Inicio"
                                                Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>
                                        </td>
                                        <td style="width:20%;text-align:left;font-weight:bold;">
                                            &nbsp;&nbsp;*Fin Licencia
                                        </td>
                                        <td style="width:30%;text-align:left;">
                                            <asp:TextBox ID="Txt_Termino_Licencia" runat="server" Width="85%" onblur='this.value = this.value.toLowerCase();'/>
                                             <cc1:CalendarExtender ID="Ce_Termino_Licencia" 
                                                runat="server" TargetControlID="Txt_Termino_Licencia" 
                                                Format="dd/MMM/yyyy"  PopupButtonID="Img_Termino_Licencia"  />
                                             <asp:ImageButton ID="Img_Termino_Licencia" runat="server" Width="10%" 
                                                ImageUrl="../imagenes/paginas/SmallCalendar.gif" style="vertical-align:top;"
                                                Height="18px" CausesValidation="false"/> 
                                            <cc1:MaskedEditExtender 
                                                ID="Mee_Termino_Licencia" 
                                                Mask="99/LLL/9999" 
                                                runat="server"
                                                MaskType="None" 
                                                UserDateFormat="DayMonthYear" 
                                                UserTimeFormat="None" Filtered="/"
                                                TargetControlID="Txt_Termino_Licencia" 
                                                Enabled="True" 
                                                ClearMaskOnLostFocus="false"/>  
                                            <cc1:MaskedEditValidator 
                                                ID="MaskedEditValidator1" 
                                                runat="server" 
                                                ControlToValidate="Txt_Termino_Licencia"
                                                ControlExtender="Mee_Termino_Licencia" 
                                                EmptyValueMessage=""
                                                InvalidValueMessage="Fecha Termino Invalida" 
                                                IsValidEmpty="true" 
                                                TooltipMessage="Ingrese o Seleccione la Fecha Termino Licencia"
                                                Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;vertical-align:top;font-weight:bold;">
                                            *Motivo Baja
                                        </td>
                                        <td colspan="3" style="width:80%;text-align:left;">
                                            <asp:TextBox ID="Txt_Comentarios_Empleado" runat="server" TabIndex="10" MaxLength="250" onkeyup='this.value = this.value.toUpperCase();'
                                                TextMode="MultiLine" Width="100%" Height="100px" />
                                            <span id="Contador_Caracteres_Comentarios" class="watermarked"></span>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Comentarios_Empleado" runat="server" 
                                                TargetControlID="Txt_Comentarios_Empleado" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ_-{}/\ "/>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>

            <cc1:ModalPopupExtender ID="Mpe_Baja_Empleado" runat="server" BackgroundCssClass="popUpStyle"  BehaviorID="MPE_Baja"
                PopupControlID="Pnl_Baja_Empleado" TargetControlID="Btn_Comodin_Cancelacion_Open" PopupDragHandleControlID="Pnl_Cabecera" 
                CancelControlID="Btn_Comodin_Cancelacion_Close" DropShadow="True" DynamicServicePath="" Enabled="True" />  
            <asp:Button Style="background-color: transparent; border-style:none;" ID="Btn_Comodin_Cancelacion_Open" runat="server" Text="" />
            <asp:Button Style="background-color: transparent; border-style:none;" ID="Btn_Comodin_Cancelacion_Close" runat="server" Text="" />


            <asp:Button ID="Btn_Comodin_MPE_Empleados" runat="server" Text="" style="display:none;"/>
            <cc1:ModalPopupExtender ID="MPE_Empleados" runat="server"  BehaviorID="Busqueda_Empleados"
            TargetControlID="Btn_Comodin_MPE_Empleados" PopupControlID="Pnl_Busqueda_Contenedor" 
            CancelControlID="Btn_Cerrar_Ventana" PopupDragHandleControlID="Pnl_Busqueda_Empleado"
            DropShadow="True" BackgroundCssClass="progressBackgroundFilter"/>  
        
    </ContentTemplate>
</asp:UpdatePanel>

<asp:Panel ID="Pnl_Baja_Empleado" runat="server" CssClass="drag" HorizontalAlign="Center" Width="650px" 
    style="display:none;border-style:outset;border-color:Silver;background-image:url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG);background-repeat:repeat-y;">                         
    <asp:Panel ID="Pnl_Cabecera" runat="server" 
        style="cursor: move;background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
        <table width="99%">
            <tr>
                <td style="color:Black;font-size:12;font-weight:bold;">
                   <asp:Image ID="Img_Inf_Baja_Empleado" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                     Baja de Empleados
                </td>
                <td align="right" style="width:10%;">
                   <asp:ImageButton ID="Btn_Cerrar_Ventana_Cancelacion" CausesValidation="false" runat="server" style="cursor:pointer;" ToolTip="Cerrar Ventana"
                        ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png" OnClientClick="javascript:$find('MPE_Baja').hide(); return false;"/>  
                </td>
            </tr>
        </table>
    </asp:Panel>
        <asp:UpdatePanel ID="Up_Popup_Baja" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
            
                <asp:UpdateProgress ID="Up_Baja_Empleado" runat="server" AssociatedUpdatePanelID="Up_Popup_Baja" DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                        <div  style="background-color:Transparent;position:fixed;top:50%;left:47%;padding:10px; z-index:1002;" id="div_progress"><img alt="" src="../Imagenes/paginas/Sias_Roler.gif" /></div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <div style="cursor:default;width:100%">
                <table width="100%">
                    <tr>
                        <td style="width:13%;vertical-align:top;top:20px;" align="center">
                            <br />
                            <asp:Image ID="Img_Empleado_Eliminar" runat="server"  Width="75px" Height="90px"
                                         ImageUrl="~/paginas/imagenes/paginas/Sias_No_Disponible.JPG" BorderWidth="1px"/> 
                        </td>
                        <td style="width:87%;">
                            <table width="100%" style="background-color:#ffffff;">
                                <tr>
                                    <td style="width:100%" colspan="4" align="left">
                                        
                                    </td>
                                </tr>
                               <tr>
                                    <td style="width:100%" colspan="4">
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:20%;text-align:left;">
                                        No Empleado
                                    </td>
                                    <td style="width:30%;text-align:left;">
                                        <asp:TextBox ID="Txt_Baja_No_Empleado" runat="server" Width="98%" MaxLength="10" TabIndex="11"
                                            Enabled="false"/>
                                        <cc1:FilteredTextBoxExtender ID="Fte_Txt_Baja_No_Empleado" 
                                             runat="server" TargetControlID="Txt_Baja_No_Empleado" FilterType="Numbers"/>
                                    </td>
                                    <td style="width:20%;text-align:left;">
                                    </td>
                                    <td style="width:30%;text-align:left;">
                                    </td>
                                </tr> 
                                <tr>
                                    <td style="width:20%;text-align:left;">
                                        Nombre
                                    </td>
                                    <td style="width:80%;text-align:left;" colspan="3">
                                        <asp:TextBox ID="Txt_Baja_Nombre_Empleado" runat="server" Width="98%" TabIndex="13" onkeyup='this.value = this.value.toUpperCase();'
                                            Enabled="false"/>
                                    </td>
                                </tr>
                                <tr>
                                   <td style="text-align:left;width:20%;vertical-align:top;">
                                       *Motivo Baja
                                   </td>
                                   <td  style="text-align:left;width:80%;" colspan="3">
                                        <asp:TextBox ID="Txt_Motivo_Baja" runat="server" Width="99%" MaxLength="100" TextMode="MultiLine" Enabled="true" 
                                            onkeyup='this.value = this.value.toUpperCase();' Height="100px"/>
                                        <cc1:FilteredTextBoxExtender ID="Fte_Txt_Motivo_Baja" runat="server"  
                                            TargetControlID="Txt_Motivo_Baja" FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers"
                                            ValidChars="áéíóúÁÉÍÓÚ ñÑ"/>
                                  </td>
                               </tr>   
                               <tr>
                                    <td style="width:100%" colspan="4">
                                        <hr />
                                    </td>
                                </tr>
                               <tr>
                                    <td style="width:100%" colspan="4" align="right">
                                        <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar" ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"
                                            style="border-style:none;background-color:White;font-weight:bold;cursor:hand;"  TabIndex="16" 
                                            OnClick="Btn_Eliminar_Click" CausesValidation="false"/>   
                                    </td>
                                </tr>
                               <tr>
                                    <td style="width:100%" colspan="4">
                                        <hr />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="Btn_Eliminar_Mostrar_Popup" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
</asp:Panel>

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


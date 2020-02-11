<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Nom_Empleados_No_Nominales.aspx.cs" Inherits="paginas_Empleados_Frm_Cat_Nom_Empleados_No_Nominales"
     Title="Catálogo Empleados No Nominales" Culture="auto" UICulture="auto"%>
     
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
    <script type="text/javascript" language="javascript">
        function calendarShown(sender, args){
            sender._popupBehavior._element.style.zIndex = 10000005;
        }
        
        function Format_Number(Nuevo_Formato, Valor_Original){
            var Ceros='';
            
            for(i=0;i<(6-Valor_Original.length);i++){
                Ceros+='0';
            }
            
            Nuevo_Formato.value = Ceros + Valor_Original;
        }

        function Generar_CONFRONTO() {
            var CONFRONTO = "";
            var FULL_NAME = "";
            var i = 0;

            try {
                FULL_NAME = $('input[id$=Txt_Apellido_Paterno_Empleado]').val() + " " +
                            $('input[id$=Txt_Apellido_Materno_Empleado]').val() + " " +
                            $('input[id$=Txt_Nombre_Empleado]').val();

                if (FULL_NAME.replace(/^\s*|\s*$/g, "") != "") {
                    FULL_NAME = FULL_NAME.toUpperCase().toString().replace(/^\s*|\s*$/g, "");
                    var Palabras = FULL_NAME.split(' ');

                    for (i = 0; i < Palabras.length; i++) {
                        CONFRONTO += Palabras[i].substring(0, 1);
                    }

                    $('input[id$=Txt_Empleado_Confronto]').css("display", "Block");
                    $('input[id$=Txt_Empleado_Confronto]').val(CONFRONTO);
                    $('input[id$=Txt_Empleado_Confronto]').css("color", "#000000");
                    $('input[id$=Txt_Empleado_Confronto]').css("font-size", "15px");
                    $('input[id$=Txt_Empleado_Confronto]').css("font-family", "Elephant");
                    $('input[id$=Txt_Empleado_Confronto]').css("text-align", "Center");
                    $('input[id$=Txt_Empleado_Confronto]').css("background-color", "#f5f5f5");
                    $('input[id$=Txt_Empleado_Confronto]').css('border-style', 'solid');
                    $('input[id$=Txt_Empleado_Confronto]').css('border-color', 'Black');
                    $('input[id$=Txt_Empleado_Confronto]').css('height', 20);

                } else {
                    $('input[id$=Txt_Empleado_Confronto]').val('');
                    $('input[id$=Txt_Empleado_Confronto]').css("display", "none");
                }
            } catch (e) {
                alert("Error al generar el CONFRONTO del empleado. Error: " + e + "]");
            }
        }
        function pageLoad() {
            $('input[id$=Txt_Empleado_Confronto]').css("display", "none");
            Generar_CONFRONTO();
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">

<asp:ScriptManager ID="Sm_Empleado_No_Nominales_Sistema" runat="server" EnableScriptGlobalization = "true" EnableScriptLocalization = "True"/>
<asp:UpdatePanel ID="Upnl_Empleado_No_Nominales_Sistema" runat="server" UpdateMode="Conditional">
    <ContentTemplate>

        <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upnl_Empleado_No_Nominales_Sistema" DisplayAfter="0">
            <ProgressTemplate>
                <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Indicador_Presidencia.gif" /></div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        
        <div id="Div_Empleados_No_Nominales" style="background-color:#ffffff; width:100%; height:100%;font-size:10px;" >
        
            <table width="100%" class="estilo_fuente">
                <tr align="center">
                    <td colspan="4" class="label_titulo">
                        Alta Empleados No Nominales
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
                                        <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button" TabIndex="1"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" onclick="Btn_Nuevo_Click" />
                                        <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button" TabIndex="2"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" onclick="Btn_Modificar_Click" />
                                        <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar" CssClass="Img_Button" TabIndex="3"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"
                                            OnClick="Btn_Eliminar_Click"/>
                                        <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" TabIndex="4"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" onclick="Btn_Salir_Click"/>
                                  </td>
                                  <td align="right" style="width:41%;">
                                    <table style="width:100%;height:28px;">
                                        <tr>
                                            <td style="width:100%;vertical-align:top;" align="right">
                                                <asp:ImageButton ID="Btn_Mostrar_Popup_Busqueda" runat="server" ToolTip="Busqueda Avanzada" TabIndex="23" 
                                                    ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="24px" Width="24px"
                                                    OnClientClick="javascript: $find('Busqueda_Empleados').show(); return false;" CausesValidation="false" />
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

           <table style="width:98%;">
                <tr>
                    <td style="width:100%;">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="width:100%;">
                        <table width="100%" class="estilo_fuente">
                            <tr>
                                <td style="width:20%;text-align:left;">
                                    Empleado ID
                                </td>
                                <td style="width:30%;text-align:left;">
                                    <asp:TextBox ID="Txt_Empleado_ID" runat="server" ReadOnly="True" Width="98%" 
                                        Enabled="false"/>
                                </td>
                                <td style="width:20%;text-align:left;">

                                </td>
                                <td style="width:30%;text-align:left;">
                                  <asp:TextBox ID="Txt_Empleado_Confronto" runat="server" Width="98%"
                                    style="
                                         background-color:Window;
                                         cursor:auto;
                                         font-family :Consolas;
                                         font-size:small;
                                         font-weight:bold;
                                         border-style:ridge;
                                         "/>
                                </td>                                
                            </tr>
                            <tr>
                                <td style="width:20%;text-align:left;">
                                    *No Empleado
                                </td>
                                <td style="width:30%;text-align:left;">
                                    <asp:TextBox ID="Txt_No_Empleado" runat="server" Width="98%" TabIndex="7" MaxLength="6"
                                        onblur='javascript:Format_Number(this, this.value);'/> 
                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_No_Empleadoo" runat="server"
                                        TargetControlID="Txt_No_Empleado" FilterType="Numbers"/>
                                </td>
                                <td style="width:20%;text-align:left;">
                                    &nbsp;&nbsp;*Estatus
                                </td>
                                <td style="width:30%;text-align:left;">
                                    <asp:DropDownList ID="Cmb_Estatus_Empleado" runat="server" Width="100%" TabIndex="8">                                                                  
                                        <asp:ListItem>&lt; - Seleccione - &gt;</asp:ListItem>
                                        <asp:ListItem>ACTIVO</asp:ListItem>
                                        <asp:ListItem>INACTIVO</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width:20%;text-align:left;">
                                    *Nombre
                                </td>
                                <td style="width:30%;text-align:left;">
                                    <asp:TextBox ID="Txt_Nombre_Empleado" runat="server" TabIndex="9" MaxLength="100" Width="98%"
                                        onkeyup="javascript:Generar_CONFRONTO();this.value = this.value.toUpperCase();"/>
                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Nombre_Empleado" runat="server" TargetControlID="Txt_Nombre_Empleado" 
                                        FilterType="Custom, UppercaseLetters, LowercaseLetters" ValidChars="ÑñáéíóúÁÉÍÓÚ "/>
                                </td>
                                <td style="width:20%;text-align:left;">
                                    &nbsp;&nbsp;*Rol
                                </td>
                                <td colspan="3" style="width:30%;text-align:left;">
                                    <asp:DropDownList ID="Cmb_Roles_Empleado" runat="server" Width="100%" TabIndex="12"/>
                                </td>
                            </tr>
                            <tr>
                                <td style="width:20%;text-align:left;">
                                    *Apellido Paterno
                                </td>
                                <td style="width:30%;text-align:left;">
                                    <asp:TextBox ID="Txt_Apellido_Paterno_Empleado" runat="server" TabIndex="10" MaxLength="100" Width="98%"
                                        onkeyup="javascript:Generar_CONFRONTO();this.value = this.value.toUpperCase();"/>
                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Apellido_Paterno_Empleado" runat="server" 
                                        TargetControlID="Txt_Apellido_Paterno_Empleado" FilterType="Custom, UppercaseLetters, LowercaseLetters" 
                                        ValidChars="ÑñáéíóúÁÉÍÓÚ "/>
                                </td>
                                <td style="width:20%;text-align:left;">
                                    &nbsp;&nbsp;Apellido Materno
                                </td>
                                <td style="width:30%;text-align:left;">
                                    <asp:TextBox ID="Txt_Apellido_Materno_Empleado" runat="server" TabIndex="11" MaxLength="100" Width="98%"
                                        onkeyup="javascript:Generar_CONFRONTO();this.value = this.value.toUpperCase();"/>
                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Apellido_Materno_Empleado" runat="server" 
                                        TargetControlID="Txt_Apellido_Materno_Empleado" FilterType="Custom, UppercaseLetters, LowercaseLetters" 
                                        ValidChars="ÑñáéíóúÁÉÍÓÚ "/>
                                </td>
                            </tr>
                            <tr>
                                <td style="width:20%;text-align:left;">
                                    *Password
                                </td>
                                <td style="width:30%;text-align:left;">
                                    <asp:TextBox ID="Txt_Password_Empleado" runat="server" TabIndex="13" MaxLength="100" 
                                        Width="98%" TextMode="Password"/>
                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Password_Empleado" runat="server" 
                                        TargetControlID="Txt_Password_Empleado" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                        ValidChars="ÑñáéíóúÁÉÍÓÚ!¡#$%&/()=¿?.,:;-+* "/>
                                </td>
                                <td style="width:20%;text-align:left;">
                                    &nbsp;&nbsp;*Confirmar Password
                                </td>
                                <td style="width:30%;text-align:left;">
                                    <asp:TextBox ID="Txt_Confirma_Password_Empleado" runat="server" TabIndex="14" MaxLength="100" 
                                        Width="98%" TextMode="Password"/>
                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Confirma_Password_Empleado" runat="server" 
                                        TargetControlID="Txt_Confirma_Password_Empleado" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                        ValidChars="ÑñáéíóúÁÉÍÓÚ!¡#$%&/()=¿?.,:;-+* " />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
           </table>

           <table style="width:98%;" class="estilo_fuente">
                <tr>
                    <td style="width:20%;text-align:left;">
                        *F. Nacimiento
                    </td>
                    <td style="width:30%;text-align:left;">
                        <asp:TextBox ID="Txt_Fecha_Nacimiento_Empleado" runat="server" Width="84%" TabIndex="16" MaxLength="11" Height="18px" 
                            onblur="this.value = (this.value.match(/^(([0-9])|([0-2][0-9])|([3][0-1]))\/(ene|feb|mar|abr|may|jun|jul|ago|sep|oct|nov|dic)\/\d{4}$/))? this.value : '';"/>
                        <cc1:TextBoxWatermarkExtender ID="TBWE_Txt_Fecha_Nacimiento_Empleado" runat="server" 
                            TargetControlID="Txt_Fecha_Nacimiento_Empleado" WatermarkCssClass="watermarked" 
                            WatermarkText="Dia/Mes/Año" Enabled="True" />
                        <cc1:CalendarExtender ID="CE_Txt_Fecha_Nacimiento_Empleado" runat="server" 
                            TargetControlID="Txt_Fecha_Nacimiento_Empleado" Format="dd/MMM/yyyy" Enabled="True" PopupButtonID="Btn_Fecha_Nacimiento"/>
                         <asp:ImageButton ID="Btn_Fecha_Nacimiento" runat="server"
                            ImageUrl="../imagenes/paginas/SmallCalendar.gif" style="vertical-align:top;"
                            Height="18px" CausesValidation="false"/>
                        <cc1:MaskedEditExtender 
                            ID="Mee_Txt_Fecha_Nacimiento_Empleado" 
                            Mask="99/LLL/9999" 
                            runat="server"
                            MaskType="None" 
                            UserDateFormat="DayMonthYear" 
                            UserTimeFormat="None" Filtered="/"
                            TargetControlID="Txt_Fecha_Nacimiento_Empleado" 
                            Enabled="True" 
                            ClearMaskOnLostFocus="false"/>  
                        <cc1:MaskedEditValidator 
                            ID="Mev_Txt_Fecha_Nacimiento_Empleado" 
                            runat="server" 
                            ControlToValidate="Txt_Fecha_Nacimiento_Empleado"
                            ControlExtender="Mee_Txt_Fecha_Nacimiento_Empleado" 
                            EmptyValueMessage="Fecha Requerida"
                            InvalidValueMessage="Fecha Nacimiento Invalida" 
                            IsValidEmpty="false" 
                            TooltipMessage="Ingrese o Seleccione la Fecha Nacimiento"
                            Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>                                                                                            
                    </td>
                    <td style="width:20%;text-align:left;">
                        &nbsp;&nbsp;*Sexo
                    </td>
                    <td style="width:30%;text-align:left;">
                        <asp:DropDownList ID="Cmb_Sexo_Empleado" runat="server" Width="100%" TabIndex="17">
                            <asp:ListItem><- Seleccionar -></asp:ListItem>
                            <asp:ListItem>MASCULINO</asp:ListItem>
                            <asp:ListItem>FEMENINO</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="width:20%;text-align:left;">
                        *RFC
                    </td>
                    <td style="width:30%;text-align:left;">
                        <asp:TextBox ID="Txt_RFC_Empleado" runat="server" Width="98%" TabIndex="18" MaxLength="20" onkeyup='this.value = this.value.toUpperCase();'
                            onblur="this.value = (this.value.match(/^[a-zA-Z]{3,4}(\d{6})((\D|\d){3})?$/))? this.value : '';"/>
                    </td>
                    <td style="width:20%;text-align:left;">
                        &nbsp;&nbsp;C.U.R.P.
                    </td>
                    <td style="width:30%;text-align:left;">
                        <asp:TextBox ID="Txt_CURP_Empleado" runat="server" Width="98%" TabIndex="19" MaxLength="20" onkeyup='this.value = this.value.toUpperCase();'
                            onblur="this.value = (this.value.match(/^[a-zA-Z]{4}(\d{6})([a-zA-Z]{6})(\d{2})?$/))? this.value : '';"/>
                    </td>
                </tr>
                <tr>
                    <td style="width:20%;text-align:left;">
                        *Domicilio
                    </td>
                    <td colspan="3" style="width:80%;text-align:left;">
                        <asp:TextBox ID="Txt_Domicilio_Empleado" runat="server" Width="99.5%" TabIndex="20" MaxLength="100"
                            onkeyup='this.value = this.value.toUpperCase();'/>
                    </td>
                </tr>
                <tr>
                    <td style="width:20%;text-align:left;">
                        *Colonia
                    </td>
                    <td style="width:30%;text-align:left;">
                        <asp:TextBox ID="Txt_Colonia_Empleado" runat="server" Width="98%" TabIndex="21" MaxLength="100"
                            onkeyup='this.value = this.value.toUpperCase();'/>
                    </td>
                    <td style="width:20%;text-align:left;">
                        &nbsp;&nbsp;CP
                    </td>
                    <td style="width:30%;text-align:left;">
                        <asp:TextBox ID="Txt_Codigo_Postal_Empleado" runat="server" Width="98%" TabIndex="22" MaxLength="5"
                            onblur="this.value = (this.value.match(/^([1-9]{2}|[0-9][1-9]|[1-9][0-9])[0-9]{3}$/))? this.value : '';"/>
                    </td>
                </tr>
                <tr>
                    <td style="width:20%;text-align:left;">
                        *Ciudad
                    </td>
                    <td style="width:30%;text-align:left;">
                        <asp:TextBox ID="Txt_Ciudad_Empleado" runat="server" Width="98%" TabIndex="23" MaxLength="50"
                            onkeyup='this.value = this.value.toUpperCase();'/>
                    </td>
                    <td style="width:20%;text-align:left;">
                        &nbsp;&nbsp;*Estado
                    </td>
                    <td style="width:30%;text-align:left;">
                        <asp:TextBox ID="Txt_Estado_Empleado" runat="server" Width="98%" TabIndex="24" MaxLength="50"
                            onkeyup='this.value = this.value.toUpperCase();'/>
                    </td>
                </tr>
                <tr>
                <tr>
                    <td style="width:20%;text-align:left;">
                        *Unidad Responsable
                    </td>
                    <td style="width:30%;text-align:left;">
                        <asp:DropDownList ID="Cmb_SAP_Unidad_Responsable" runat="server" Width="100%" AutoPostBack="true"
                            OnSelectedIndexChanged="Cmb_SAP_Unidad_Responsable_SelectedIndexChanged"/>
                    </td>
                    <td style="width:20%;text-align:left;">
                        &nbsp;&nbsp;*Area
                    </td>
                    <td style="width:30%;text-align:left;">
                        <asp:DropDownList ID="Cmb_Areas_Empleado" runat="server" TabIndex="33" Width="100%" />
                    </td>
                </tr>
                <tr>
                    <td style="width:20%;text-align:left;vertical-align:top;">
                        Comentarios
                    </td>
                    <td colspan="3" style="width:80%;text-align:left;">
                        <asp:TextBox ID="Txt_Comentarios_Empleado" runat="server" TabIndex="15" MaxLength="250"
                            TextMode="MultiLine" Width="100%" Height="60px"/>
                        <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Comentarios_Empleado" runat="server" 
                            TargetControlID ="Txt_Comentarios_Empleado" WatermarkText="Límite de Caractes 250" 
                            WatermarkCssClass="watermarked"/>
                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Comentarios_Empleado" runat="server" 
                            TargetControlID="Txt_Comentarios_Empleado" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                            ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ "/>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="width:100%;text-align:left;">
                        <hr />
                    </td>
                </tr>
            </table>
            
            <asp:GridView ID="Grid_Empleados" runat="server" AllowPaging="True" CssClass="GridView_1" 
                AutoGenerateColumns="False" PageSize="10" GridLines="None" Width="98.5%"
                onpageindexchanging="Grid_Empleados_PageIndexChanging"  
                onselectedindexchanged="Grid_Empleados_SelectedIndexChanged" HeaderStyle-CssClass="tblHead">
                
                <Columns>
                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                        <ItemStyle Width="7%" />
                    </asp:ButtonField>
                    <asp:BoundField DataField="Empleado_ID" HeaderText="Empleado ID">
                        <HeaderStyle HorizontalAlign="Left" Width="0%" />
                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="No_Empleado" HeaderText="No Empleado">
                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="RFC" HeaderText="RFC">
                        <HeaderStyle HorizontalAlign="Left" Width="0%" />
                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Empleado" HeaderText="Nombre" 
                        Visible="True" SortExpression="Empleado">
                        <HeaderStyle HorizontalAlign="Left" Width="78%" />
                        <ItemStyle HorizontalAlign="left" Width="78%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Estatus" HeaderText="Estatus">
                        <HeaderStyle HorizontalAlign="Left" Width="0%" />
                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                    </asp:BoundField>
                </Columns>
                <SelectedRowStyle CssClass="GridSelected" />
                <PagerStyle CssClass="GridHeader" />
                <AlternatingRowStyle CssClass="GridAltItem" />
            </asp:GridView>
            
            <cc1:ModalPopupExtender 
                ID="Mpe_Busqueda_Empleados" 
                runat="server" 
                BackgroundCssClass="popUpStyle" 
                BehaviorID="Busqueda_Empleados"
                PopupControlID="Pnl_Busqueda_Contenedor" 
                TargetControlID="Btn_Comodin_Open" 
                PopupDragHandleControlID="Pnl_Busqueda_Cabecera" 
                CancelControlID="Btn_Comodin_Close" 
                DropShadow="True" 
                DynamicServicePath="" 
                Enabled="True"/>
                
            <asp:Button Style="background-color: transparent; border-style:none;" ID="Btn_Comodin_Close" runat="server" Text="" />
            <asp:Button  Style="background-color: transparent; border-style:none;" ID="Btn_Comodin_Open" runat="server" Text="" />
        </ContentTemplate>
    </asp:UpdatePanel>

 <asp:Panel ID="Pnl_Busqueda_Contenedor" runat="server" CssClass="drag"  HorizontalAlign="Center" Width="650px" 
    style="display:none;border-style:outset;border-color:Silver;background-image:url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG);background-repeat:repeat-y;">
    <asp:Panel ID="Pnl_Busqueda_Cabecera" runat="server" 
        style="cursor: move;background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
        <table width="99%">
            <tr>
                <td style="color:Black;font-size:12;font-weight:bold;">
                   <asp:Image ID="Img_Informatcion_Autorizacion" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                     B&uacute;squeda: Empleados
                </td>
                <td align="right" style="width:10%;">
                   <asp:ImageButton ID="Btn_Cerrar_Ventana" CausesValidation="false" runat="server" style="cursor:pointer;" ToolTip="Cerrar Ventana"
                        ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png" OnClientClick="javascript: $find('Busqueda_Empleados').hide(); return false;"/>
                </td>
            </tr>
        </table>
    </asp:Panel>
           <div style="color: #5D7B9D">
             <table width="100%">
                <tr>
                    <td align="left" style="text-align: left;" >
                        <asp:UpdatePanel ID="Upnl_Filtros_Busqueda_Prestamos" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                            
                                <asp:UpdateProgress ID="Progress_Upnl_Filtros_Busqueda_Prestamos" runat="server" AssociatedUpdatePanelID="Upnl_Filtros_Busqueda_Prestamos" DisplayAfter="0">
                                    <ProgressTemplate>
                                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                                        <div  style="background-color:Transparent;position:fixed;top:50%;left:47%;padding:10px; z-index:1002;" id="div_progress"><img alt="" src="../Imagenes/paginas/Sias_Roler.gif" /></div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                                
                                  <table width="100%">
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
                                           <asp:TextBox ID="Txt_Busqueda_No_Empleado" runat="server" Width="98%" 
                                                onblur='javascript:Format_Number(this, this.value);' MaxLength="6"/>
                                           <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_No_Empleado" runat="server" FilterType="Numbers"
                                                TargetControlID="Txt_Busqueda_No_Empleado"/>
                                            <cc1:TextBoxWatermarkExtender ID="Twm_Txt_Busqueda_No_Empleado" runat="server" 
                                                TargetControlID ="Txt_Busqueda_No_Empleado" WatermarkText="Busqueda por No Empleado" 
                                                WatermarkCssClass="watermarked"/>
                                        </td> 
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                        </td>
                                        <td style="width:30%;text-align:left;font-size:11px;">
                                        </td>
                                    </tr>
                                    <tr>
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
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            Estatus
                                        </td>
                                        <td style="width:30%;text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Estatus" runat="server" Width="100%">
                                                <asp:ListItem Value="">&lt;- Seleccione -&gt;</asp:ListItem>
                                                <asp:ListItem>ACTIVO</asp:ListItem>
                                                <asp:ListItem>INACTIVO</asp:ListItem>
                                            </asp:DropDownList>
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
                                            Rol
                                        </td>
                                        <td style="width:30%;text-align:left;font-size:11px;" colspan="3">
                                           <asp:DropDownList ID="Cmb_Busqueda_Rol" runat="server" Width="100%" />
                                        </td> 
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            Unidad Responsable
                                        </td>              
                                        <td style="width:30%;text-align:left;font-size:11px;" colspan="3">
                                           <asp:DropDownList ID="Cmb_Busqueda_Dependencia" runat="server" Width="100%" 
                                                AutoPostBack="true" OnSelectedIndexChanged="Cmb_Busqueda_Dependencia_SelectedIndexChanged"/>
                                        </td> 
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            Area
                                        </td>
                                        <td style="width:30%;text-align:left;font-size:11px;" colspan="3">
                                           <asp:DropDownList ID="Cmb_Busqueda_Areas" runat="server" Width="100%" />
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
                                                CausesValidation="false" OnClick="Btn_Busqueda_Empleados_Click" Width="200px" OnClientClick="javascript: $find('Busqueda_Empleados').hide();"/> 
                                            </center>
                                        </td>
                                    </tr>
                                  </table>
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


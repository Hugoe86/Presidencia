<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Nom_Parametros_Nomina.aspx.cs" Inherits="paginas_Nomina_Frm_Cat_Nom_Parametros_Nomina" Title="Catálogo Parámetros Nómina" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
    <script src="../../easyui/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script src="../../easyui/jquery.formatCurrency-1.4.0.min.js" type="text/javascript"></script>
    <script src="../../easyui/jquery.formatCurrency.all.js" type="text/javascript"></script>

    <link href="../estilos/estilo_paginas.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">

    <asp:ScriptManager ID="ScriptManager_Parametros_Nomina" runat="server" 
        EnableScriptGlobalization = "true" EnableScriptLocalization = "True"/>
        
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
        
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            
            <div id="Div_Calendario_Nominas" style="background-color:#ffffff; width:100%; height:100%;">
            
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="4" class="label_titulo">
                            Catálogo de Par&aacute;metros de la Nomina
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">&nbsp;
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
                                            <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" 
                                                CssClass="Img_Button" TabIndex="1"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" onclick="Btn_Nuevo_Click" 
                                                />
                                            <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" 
                                                CssClass="Img_Button" TabIndex="2"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" onclick="Btn_Modificar_Click" 
                                                /> 
                                            <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar" 
                                                CssClass="Img_Button" TabIndex="2"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"
                                                OnClientClick="return confirm('¿Está seguro de eliminar el registro seleccionado?');" 
                                                onclick="Btn_Eliminar_Click" 
                                                />
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" 
                                                CssClass="Img_Button" TabIndex="4"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" onclick="Btn_Salir_Click" 
                                                />
                                          </td>
                                          <td align="right" style="width:41%;">
                                            <table style="width:100%;height:28px;">
                                                <tr>
                                                    <td style="vertical-align:middle;text-align:right;width:20%;">
                                                    </td>
                                                    <td style="width:55%;">
                                                    </td>
                                                    <td style="vertical-align:middle;width:5%;" >
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
                
                <cc1:TabContainer runat="server" ID="Tc_Parametros" Width="98%" CssClass="Tab">
                    <cc1:TabPanel ID="Tpnl_Parametros_1" runat="server" HeaderText="Parámetros Nómina">
                        <HeaderTemplate>
                            Parámetros Nómina
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table width="98%">
                                <tr>
                                    <td style="width:100%" colspan="4">
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:left;width:20%;">
                                        Par&aacute;metro ID
                                    </td>
                                    <td  style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Parametro_ID" runat="server" Width="98%"/>
                                    </td>
                                    <td style="text-align:left;width:20%;">
                                        Tope ISSEG
                                    </td>
                                    <td  style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Tope_ISSEG" runat="server" Width="98%" style="text-align:left;"
                                         onblur="$('input[id$=Txt_Tope_ISSEG]').formatCurrency({colorize:true, region: 'es-MX'});"/> 
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:left;width:20%;">
                                       Tipo Cálculo IMSS
                                    </td>
                                    <td  style="text-align:left;width:30%;">
                                       <asp:DropDownList ID="Cmb_Tipo_Calculo_IMSS" runat="server" Width="100%">
                                            <asp:ListItem>&lt; - Seleccione - &gt;</asp:ListItem>
                                            <asp:ListItem>ACTUAL</asp:ListItem>
                                            <asp:ListItem>NUEVO</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align:left;width:20%;">
                                        &nbsp;&nbsp;Dias IMSS  
                                    </td>
                                    <td  style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Dias_IMSS" runat="server" Width="98%"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:left;width:20%;">
                                        Minutos D&iacute;a
                                    </td>
                                    <td  style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Minutos_Dia" runat="server" Width="98%"
                                            onkeyup="$('input[id$=Txt_Minutos_Dia]').filter(function(){if(!this.value.match(/^[0-9]{0,2}\.?[0-9]{0,2}$/))$(this).val('');});"/>
                                    </td>
                                    <td style="text-align:left;width:20%;">
                                        &nbsp;&nbsp;Minutos Retardo
                                    </td>
                                    <td  style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Minutos_Retardo" runat="server" Width="98%"
                                            onkeyup="$('input[id$=Txt_Minutos_Retardo]').filter(function(){if(!this.value.match(/^[0-9]{0,2}\.?[0-9]{0,2}$/))$(this).val('');});"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:left;width:20%;">
                                        Previsi&oacute;n Social
                                    </td>
                                    <td  style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Porcentaje_Prevision_Social_Multiple" runat="server" Width="98%"
                                            onblur="$('input[id$=Txt_Porcentaje_Prevision_Social_Multiple]').filter(function(){if(!this.value.match(/(^100([.]0{1,2})?)$|(^\d{1,2}([.]\d{1,12})?)$/))$(this).val('');});"/>
                                        
                                    </td>
                                    <td style="text-align:left;width:20%;">
                                        &nbsp;&nbsp;[%] Factor Social Empleado
                                    </td>
                                    <td  style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Porcentaje_Factor_Social" runat="server" Width="98%"
                                            onblur="$('input[id$=Txt_Porcentaje_Factor_Social]').filter(function(){if(!this.value.match(/(^100([.]0{1,2})?)$|(^\d{1,2}([.]\d{1,12})?)$/))$(this).val('');});"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:left;width:20%;">
                                        *Zona Económica
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                         <asp:DropDownList ID="Cmb_Zona" runat="server" Width="100%"/>
                                    </td>
                                    <td style="text-align:left;width:20%;">
                                        &nbsp;&nbsp;*Fondo de Retiro [%] 
                                    </td>
                                    <td style="text-align:left;width:30%;"> 
                                        <asp:TextBox ID="Txt_Porcentaje_Fondo_Retiro" runat="server" Width="98%" MaxLength="15"/>
                                        <cc1:FilteredTextBoxExtender ID="FTxt_Porcentaje_Fondo_Retiro" runat="server"  
                                                TargetControlID="Txt_Porcentaje_Fondo_Retiro" FilterType="Numbers, Custom" ValidChars="."/>
                                        <asp:CustomValidator ID="Cv_Txt_Porcentaje_Fondo_Retiro" runat="server"  Display="None"
                                             EnableClientScript="true" ErrorMessage="El Porcentaje de Fondo de Retiro no puede ser menor 0 ó mayor a 100"
                                             Enabled="true"
                                             ClientValidationFunction="TextBox_Txt_Porcentaje_Fondo_Retiro"
                                             HighlightCssClass="highlight" 
                                             ControlToValidate="Txt_Porcentaje_Fondo_Retiro"/>
                                        <cc1:ValidatorCalloutExtender ID="Vce_Txt_Porcentaje_Fondo_Retiro" runat="server" 
                                            TargetControlID="Cv_Txt_Porcentaje_Fondo_Retiro" PopupPosition="TopRight"/>
                                        <script type="text/javascript" >
                                            function TextBox_Txt_Porcentaje_Fondo_Retiro(sender, args) {     
                                                 var Porcentaje_Fondo_Retiro = document.getElementById("<%=Txt_Porcentaje_Fondo_Retiro.ClientID%>").value;   
                                                 if (Porcentaje_Fondo_Retiro < 0 ||  Porcentaje_Fondo_Retiro > 100){  
                                                    document.getElementById("<%=Txt_Porcentaje_Fondo_Retiro.ClientID%>").value ="";       
                                                    args.IsValid = false;
                                                 }
                                              } 
                                        </script>
                                    </td>
                                </tr>
                               <tr>
                                    <td style="text-align:left;width:20%;">
                                       *Prima Vacacional [%] 
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Porcentaje_Prima_Vacacional" runat="server" Width="98%" MaxLength="100"/>
                                        <cc1:FilteredTextBoxExtender ID="FTxt_Porcentaje_Prima_Vacacional" runat="server"
                                            TargetControlID="Txt_Porcentaje_Prima_Vacacional" FilterType="Numbers, Custom" ValidChars="."/>
                                        <asp:CustomValidator ID="Cv_Txt_Porcentaje_Prima_Vacacional" runat="server"  Display="None"
                                             EnableClientScript="true" ErrorMessage="El Porcentaje Prima Vacacional no puede ser menor 0 ó mayor a 100"
                                             Enabled="true"
                                             ClientValidationFunction="TextBox_Txt_Porcentaje_Prima_Vacacional"
                                             HighlightCssClass="highlight" 
                                             ControlToValidate="Txt_Porcentaje_Prima_Vacacional"/>
                                        <cc1:ValidatorCalloutExtender ID="Vce_TextBox_Txt_Porcentaje_Prima_Vacacional" runat="server" 
                                            TargetControlID="Cv_Txt_Porcentaje_Prima_Vacacional" PopupPosition="TopRight"/>
                                        <script type="text/javascript" >
                                            function TextBox_Txt_Porcentaje_Prima_Vacacional(sender, args) {
                                                 var Porcentaje_Prima_Vacacional = document.getElementById("<%=Txt_Porcentaje_Prima_Vacacional.ClientID%>").value;   
                                                 if (Porcentaje_Prima_Vacacional < 0 ||  Porcentaje_Prima_Vacacional > 100){  
                                                    document.getElementById("<%=Txt_Porcentaje_Prima_Vacacional.ClientID%>").value ="";
                                                    args.IsValid = false;
                                                 }
                                              }
                                        </script>
                                    </td>
                                    <td style="text-align:left;width:20%;">
                                        &nbsp;&nbsp;*Prima Dominical [%] 
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Porcentaje_Prima_Dominical" runat="server" Width="98%" MaxLength="15"/>
                                        <cc1:FilteredTextBoxExtender ID="FTxt_Porcentaje_Prima_Dominical" runat="server"  TargetControlID="Txt_Porcentaje_Prima_Dominical"
                                            FilterType="Numbers, Custom" ValidChars="."/>
                                        <asp:CustomValidator ID="Cv_Txt_Porcentaje_Prima_Dominical" runat="server"  Display="None"
                                             EnableClientScript="true" ErrorMessage="El Porcentaje Prima Dominical no puede ser menor 0 ó mayor a 100"
                                             Enabled="true"
                                             ClientValidationFunction="TextBox_Txt_Porcentaje_Prima_Dominical"
                                             HighlightCssClass="highlight" 
                                             ControlToValidate="Txt_Porcentaje_Prima_Dominical"/>
                                        <cc1:ValidatorCalloutExtender ID="Vce_Txt_Porcentaje_Prima_Dominical" runat="server" 
                                            TargetControlID="Cv_Txt_Porcentaje_Prima_Dominical" PopupPosition="TopRight"/>
                                        <script type="text/javascript" >
                                            function TextBox_Txt_Porcentaje_Prima_Dominical(sender, args) {
                                                 var Porcentaje_Prima_Dominical= document.getElementById("<%=Txt_Porcentaje_Prima_Dominical.ClientID%>").value;
                                                 if (Porcentaje_Prima_Dominical < 0 ||  Porcentaje_Prima_Dominical > 100){  
                                                    document.getElementById("<%=Txt_Porcentaje_Prima_Dominical.ClientID%>").value ="";
                                                    args.IsValid = false;
                                                 }
                                              }
                                        </script>
                                    </td>
                                </tr>
                               <tr>
                                    <td style="text-align:left;width:20%;">
                                        *Prima Vacacional 1
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Fecha_Prima_Vacacional_1" runat="server" Width="85%" MaxLength="15"/>
                                        <cc1:FilteredTextBoxExtender ID="FTxt_Fecha_Prima_Vacacional_1" runat="server"  TargetControlID="Txt_Fecha_Prima_Vacacional_1"
                                            FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" ValidChars="/"/>
                                        <cc1:CalendarExtender ID="Txt_Fecha_Prima_Vacacional_1_CalendarExtender" runat="server" 
                                            TargetControlID="Txt_Fecha_Prima_Vacacional_1" PopupButtonID="Btn_Txt_Fecha_Prima_Vacacional_1" Format="dd/MMM/yyyy"/>
                                        <asp:ImageButton ID="Btn_Txt_Fecha_Prima_Vacacional_1" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                            ToolTip="Seleccione la Fecha Prima Vacacional 1"/>
                                    </td>
                                    <td style="text-align:left;width:20%;">
                                        &nbsp;&nbsp;* Prima Vacacional 2
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Fecha_Prima_Vacacional_2" runat="server" Width="85%" MaxLength="10"/>
                                        <cc1:FilteredTextBoxExtender ID="FTxt_Fecha_Prima_Vacacional_2" runat="server"  TargetControlID="Txt_Fecha_Prima_Vacacional_2"
                                            FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" ValidChars="/"/>  
                                        <cc1:CalendarExtender ID="Txt_Fecha_Prima_Vacacional_2_CalendarExtender" runat="server" 
                                            TargetControlID="Txt_Fecha_Prima_Vacacional_2" PopupButtonID="Btn_Txt_Fecha_Prima_Vacacional_2" Format="dd/MMM/yyyy"/>
                                        <asp:ImageButton ID="Btn_Txt_Fecha_Prima_Vacacional_2" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                            ToolTip="Seleccione la Fecha Prima Vacacional 2"/>
                                    </td>
                                </tr>
                               <tr>
                                    <td style="text-align:left;width:20%;">
                                       *Limite Prestamo
                                    </td>
                                    <td style="text-align:left;width:80%;" colspan="3">
                                        <asp:TextBox ID="Txt_Salario_Limite_Prestamo" runat="server" Width="99.5%" TabIndex="10"/>
                                        <cc1:MaskedEditExtender ID="MEE_Txt_Salario_Limite_Prestamo" runat="server"
                                            TargetControlID="Txt_Salario_Limite_Prestamo"
                                            Mask="9,999,999.99"
                                            MessageValidatorTip="true"
                                            OnFocusCssClass="MaskedEditFocus"
                                            OnInvalidCssClass="MaskedEditError"
                                            MaskType="Number"
                                            InputDirection="RightToLeft"
                                            AcceptNegative="Left"
                                            DisplayMoney="Left"
                                            ErrorTooltipEnabled="True"
                                            AutoComplete="true"
                                            AutoCompleteValue="0"
                                            ClearTextOnInvalid ="true"
                                            /> 
                                        <cc1:MaskedEditValidator
                                            ID="MEV_Txt_Salario_Limite_Prestamo" 
                                            runat="server"
                                            ControlExtender="MEE_Txt_Salario_Limite_Prestamo"
                                            ControlToValidate="Txt_Salario_Limite_Prestamo" 
                                            IsValidEmpty="false" 
                                            MaximumValue="9000000" 
                                            EmptyValueMessage="El Importe del Prestamo no puede ser $0.00"
                                            InvalidValueMessage="Formato del Importe del Prestamo  es inválido."
                                            MaximumValueMessage="Cantidad > $9,000,000.00"
                                            MinimumValueMessage="Cantidad < $0.00"
                                            MinimumValue="0" 
                                            EmptyValueBlurredText="Cantidad Requerida" 
                                            InvalidValueBlurredMessage="Formato Incorrecto" 
                                            MaximumValueBlurredMessage="Cantidad > $9,000,000.00" 
                                            MinimumValueBlurredText="Cantidad < $0.00"
                                            Display="Dynamic"
                                            TooltipMessage="Monto entre $0.00 y $9,000,000.00"
                                            style="font-size:9px;"/>
                                    </td>
                                </tr>
                               <tr>
                                    <td style="text-align:left;width:20%;">
                                        <asp:Label ID="Lbl_Salario_Mensual_Maximo" runat="server" Text="*SMM"
                                            ToolTip="Salario Mensual Máximo"/>
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Salario_Mensual_Maximo" runat="server" Width="99.5%"/>
                                        <cc1:MaskedEditExtender ID="MEE_Txt_Salario_Mensual_Maximo" runat="server"
                                            TargetControlID="Txt_Salario_Mensual_Maximo"
                                            Mask="9,999,999.99"
                                            MessageValidatorTip="true"
                                            OnFocusCssClass="MaskedEditFocus"
                                            OnInvalidCssClass="MaskedEditError"
                                            MaskType="Number"
                                            InputDirection="RightToLeft"
                                            AcceptNegative="Left"
                                            DisplayMoney="Left"
                                            ErrorTooltipEnabled="True"
                                            AutoComplete="true"
                                            AutoCompleteValue="0"
                                            ClearTextOnInvalid ="true"
                                            />
                                    </td>
                                    <td style="text-align:left;width:20%;">
                                        &nbsp;&nbsp;<asp:Label ID="Lbl_Salario_Diario_Int_Topado" runat="server" Text="*SDIT"
                                            ToolTip="Salario Diario Integrado Topado"/>
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Salario_Diario_Integrado_Topado" runat="server" Width="99.5%"/>
                                        <cc1:MaskedEditExtender ID="MEE_Txt_Salario_Diario_Integrado_Topado" runat="server"
                                            TargetControlID="Txt_Salario_Diario_Integrado_Topado"
                                            Mask="9,999,999.99"
                                            MessageValidatorTip="true"
                                            OnFocusCssClass="MaskedEditFocus"
                                            OnInvalidCssClass="MaskedEditError"
                                            MaskType="Number"
                                            InputDirection="RightToLeft"
                                            AcceptNegative="Left"
                                            DisplayMoney="Left"
                                            ErrorTooltipEnabled="True"
                                            AutoComplete="true"
                                            AutoCompleteValue="0"
                                            ClearTextOnInvalid ="true"
                                            />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:20%; text-align:left;">
                                        Proveedor Fonacot
                                    </td>
                                    <td style="width:80%;" colspan="3">
                                        <asp:DropDownList ID="Cmb_Proveedor_Fonacot" runat="server" Width="100%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:100%" colspan="4">
                                        <hr />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="Tpnl_Parámetros_2" runat="server" HeaderText="Percepciones">
                        <HeaderTemplate>
                            Parámetros Percepciones
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table width="98%">
                               <tr>
                                   <td style="width:100%" colspan="4">
                                       <hr />
                                   </td>
                               </tr>
                               <tr>
                                    <td style="text-align:left;width:30%;">
                                       *Quinquenio
                                    </td>
                                    <td style="text-align:left;width:70%;" colspan="3">
                                        <asp:DropDownList ID="Cmb_Percepcion_Quinquenio" runat="server"  Width="100%"/>
                                    </td>
                                </tr>
                               <tr>
                                    <td style="text-align:left;width:30%;">
                                       *Prima Vacacional
                                    </td>
                                    <td style="text-align:left;width:70%;" colspan="3">
                                        <asp:DropDownList ID="Cmb_Percepcion_Prima_Vacacional" runat="server"  Width="100%"/>
                                    </td>
                                </tr>
                               <tr>
                                    <td style="text-align:left;width:30%;">
                                       *Prima Dominical
                                    </td>
                                    <td style="text-align:left;width:70%;" colspan="3">
                                        <asp:DropDownList ID="Cmb_Percepcion_Prima_Dominical" runat="server"  Width="100%"/>
                                    </td>
                                </tr>
                               <tr>
                                    <td style="text-align:left;width:30%;">
                                       *Prima Antiguedad
                                    </td>
                                    <td style="text-align:left;width:70%;" colspan="3">
                                        <asp:DropDownList ID="Cmb_Percepcion_Prima_Antiguedad" runat="server"  Width="100%"/>
                                    </td>
                                </tr>
                               <tr>
                                    <td style="text-align:left;width:30%;">
                                       *Aguinaldo
                                    </td>
                                    <td style="text-align:left;width:70%;" colspan="3">
                                        <asp:DropDownList ID="Cmb_Percepcion_Aguinaldo" runat="server"  Width="100%"/>
                                    </td>
                                </tr>
                               <tr>
                                    <td style="text-align:left;width:30%;">
                                       *Dias Festivos
                                    </td>
                                    <td style="text-align:left;width:70%;" colspan="3">
                                        <asp:DropDownList ID="Cmb_Percepcion_Dias_Festivos" runat="server"  Width="100%"/>
                                    </td>
                                </tr>
                               <tr>
                                    <td style="text-align:left;width:30%;">
                                       *Horas Extra
                                    </td>
                                    <td style="text-align:left;width:70%;" colspan="3">
                                        <asp:DropDownList ID="Cmb_Percepcion_Horas_Extra" runat="server"  Width="100%"/>
                                    </td>
                                </tr>
                               <tr>
                                    <td style="text-align:left;width:30%;">
                                       *Dia Doble
                                    </td>
                                    <td style="text-align:left;width:70%;"colspan="3">
                                        <asp:DropDownList ID="Cmb_Percepcion_Dia_Doble" runat="server"  Width="100%"/>
                                    </td>
                                </tr>
                               <tr>
                                    <td style="text-align:left;width:30%;">
                                       *Dia Domingo
                                    </td>
                                    <td style="text-align:left;width:70%;" colspan="3">
                                        <asp:DropDownList ID="Cmb_Percepcion_Dia_Domingo" runat="server"  Width="100%"/>
                                    </td>
                                </tr>
                               <tr>
                                    <td style="text-align:left;width:30%;">
                                       *Ajuste ISR
                                    </td>
                                    <td style="text-align:left;width:70%;" colspan="3">
                                        <asp:DropDownList ID="Cmb_Percepcion_Ajuste_ISR" runat="server"  Width="100%"/>
                                    </td>
                                </tr>
                               <tr>
                                    <td style="text-align:left;width:30%;">
                                       *Icapacidades
                                    </td>
                                    <td style="text-align:left;width:70%;" colspan="3">
                                        <asp:DropDownList ID="Cmb_Percepcion_Incapacidades" runat="server"  Width="100%"/>
                                    </td>
                                </tr>
                               <tr>
                                    <td style="text-align:left;width:30%;">
                                       *Subsidio
                                    </td>
                                    <td style="text-align:left;width:70%;" colspan="3">
                                        <asp:DropDownList ID="Cmb_Percepcion_Subsidio" runat="server"  Width="100%"/>
                                    </td>
                                </tr>
                               <tr>
                                    <td style="text-align:left;width:30%;">
                                       *Despensa
                                    </td>
                                    <td style="text-align:left;width:70%;" colspan="3">
                                        <asp:DropDownList ID="Cmb_Percepcion_Despensa" runat="server"  Width="100%"/>
                                    </td>
                                </tr>
                               <tr>
                                    <td style="text-align:left;width:30%;">
                                       *Indemnizaci&oacute;n
                                    </td>
                                    <td style="text-align:left;width:70%;" colspan="3">
                                        <asp:DropDownList ID="Cmb_Percepcion_Indemnizacion" runat="server"  Width="100%"/>
                                    </td>
                                </tr>
                               <tr>
                                    <td style="text-align:left;width:30%;">
                                       *Sueldo Normal
                                    </td>
                                    <td style="text-align:left;width:70%;" colspan="3">
                                        <asp:DropDownList ID="Cmb_Percepcion_Sueldo_Normal" runat="server"  Width="100%"/>
                                    </td>
                                </tr>
                               <tr>
                                    <td style="text-align:left;width:30%;">
                                       *Vacaciones
                                    </td>
                                    <td style="text-align:left;width:70%;" colspan="3">
                                        <asp:DropDownList ID="Cmb_Percepcion_Vacaciones" runat="server"  Width="100%"/>
                                    </td>    
                                </tr>
                               <tr>
                                    <td style="text-align:left;width:30%;">
                                       *Vacaciones Pendientes por Pagar
                                    </td>
                                    <td style="text-align:left;width:70%;" colspan="3">
                                        <asp:DropDownList ID="Cmb_Percepcion_Vacaciones_Pendientes_Pagar" runat="server"  Width="100%"/>
                                    </td>
                                </tr>
                               <tr>
                                    <td style="text-align:left;width:30%;">
                                       *Fondo Retiro
                                    </td>
                                    <td style="text-align:left;width:70%;" colspan="3">
                                        <asp:DropDownList ID="Cmb_Percepcion_Fondo_Retiro" runat="server"  Width="100%"/>
                                    </td>
                                </tr>
                               <tr>
                                    <td style="text-align:left;width:30%;">
                                       *PSM
                                    </td>
                                    <td style="text-align:left;width:70%;" colspan="3">
                                        <asp:DropDownList ID="Cmb_Percepcion_PSM" runat="server"  Width="100%"/>
                                    </td>
                                </tr>
                               <tr>
                                   <td style="width:100%" colspan="4">
                                       <hr />
                                   </td>
                               </tr>
                            </table>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="Tpnl_Deducciones" runat="server" HeaderText="Deducciones">
                        <HeaderTemplate>
                            Parámetros Deducciones
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table width="98%">
                               <tr>
                                   <td style="width:100%" colspan="4">
                                       <hr />
                                   </td>
                               </tr>
                               <tr>
                                    <td style="text-align:left;width:30%;">
                                       *Faltas
                                    </td>
                                    <td style="text-align:left;width:70%;" colspan="3">
                                        <asp:DropDownList ID="Cmb_Deduccion_Faltas" runat="server"  Width="100%"/>
                                    </td>
                                </tr>
                               <tr>
                                    <td style="text-align:left;width:30%;">
                                       *Retardos
                                    </td>
                                    <td style="text-align:left;width:70%;" colspan="3">
                                        <asp:DropDownList ID="Cmb_Deduccion_Retardos" runat="server"  Width="100%"/>
                                    </td>
                                </tr>
                               <tr>
                                    <td style="text-align:left;width:30%;">
                                       *Fondo Retiro
                                    </td>
                                    <td style="text-align:left;width:70%;" colspan="3">
                                        <asp:DropDownList ID="Cmb_Deduccion_Fondo_Retiro" runat="server"  Width="100%"/>
                                    </td>
                                </tr>
                               <tr>
                                    <td style="text-align:left;width:30%;">
                                       *ISR
                                    </td>
                                    <td style="text-align:left;width:70%;" colspan="3">
                                        <asp:DropDownList ID="Cmb_Deduccion_ISR" runat="server"  Width="100%"/>
                                    </td>
                                </tr>
                               <tr>
                                    <td style="text-align:left;width:30%;">
                                       *OJ Sueldo
                                    </td>
                                    <td style="text-align:left;width:70%;" colspan="3">
                                        <asp:DropDownList ID="Cmb_Deduccion_Orden_Judicial" runat="server"  Width="100%"/>
                                    </td>
                                </tr>
                               <tr>
                                    <td style="text-align:left;width:30%;">
                                       *OJ Aguinaldo
                                    </td>
                                    <td style="text-align:left;width:70%;" colspan="3">
                                        <asp:DropDownList ID="Cmb_Deduccion_Orden_Judicial_Aguinaldo" runat="server"  Width="100%"/>
                                    </td>
                                </tr>
                               <tr>
                                    <td style="text-align:left;width:30%;">
                                       *OJ Prima Vacacional
                                    </td>
                                    <td style="text-align:left;width:70%;" colspan="3">
                                        <asp:DropDownList ID="Cmb_Deduccion_Orden_Judicial_Prima_Vacacional" runat="server"  Width="100%"/>
                                    </td>
                                </tr>
                               <tr>
                                    <td style="text-align:left;width:30%;">
                                       *OJ Indemnización
                                    </td>
                                    <td style="text-align:left;width:70%;" colspan="3">
                                        <asp:DropDownList ID="Cmb_Deduccion_Orden_Judicial_Indemnizacion" runat="server"  Width="100%"/>
                                    </td>
                                </tr>
                               <tr>
                                    <td style="text-align:left;width:30%;">
                                       *IMSS
                                    </td>
                                    <td style="text-align:left;width:70%;" colspan="3">
                                        <asp:DropDownList ID="Cmb_Deduccion_IMSS" runat="server"  Width="100%"/>
                                    </td>
                                </tr>
                               <tr>
                                    <td style="text-align:left;width:30%;">
                                       *ISSEG
                                    </td>
                                    <td style="text-align:left;width:70%;" colspan="3">
                                        <asp:DropDownList ID="Cmb_Deduccion_ISSEG" runat="server"  Width="100%"/>
                                    </td>
                                </tr>
                               <tr>
                                    <td style="text-align:left;width:30%;">
                                       *Vacaciones Tomadas Mas
                                    </td>
                                    <td style="text-align:left;width:70%;" colspan="3">
                                        <asp:DropDownList ID="Cmb_Deduccion_Vacaciones_Tomadas_Mas" runat="server"  Width="100%"/>
                                    </td>
                                </tr>
                               <tr>
                                    <td style="text-align:left;width:30%;">
                                       *Aguinaldo Pagado de Mas
                                    </td>
                                    <td style="text-align:left;width:70%;" colspan="3">
                                        <asp:DropDownList ID="Cmb_Deduccion_Aguinaldo_Pagado_Mas" runat="server"  Width="100%"/>
                                    </td>
                                </tr>
                               <tr>
                                    <td style="text-align:left;width:30%;">
                                       *Prima Vacacional de Pagada Mas
                                    </td>
                                    <td style="text-align:left;width:80%;" colspan="3">
                                        <asp:DropDownList ID="Cmb_Deduccion_PV_Pagada_Mas" runat="server"  Width="100%"/>
                                    </td>    
                                </tr>
                               <tr>
                                    <td style="text-align:left;width:30%;">
                                       *Sueldo Pagado de Mas
                                    </td>
                                    <td style="text-align:left;width:70%;" colspan="3">
                                        <asp:DropDownList ID="Cmb_Deduccion_Sueldo_Pagado_Mas" runat="server"  Width="100%"/>
                                    </td>
                                </tr>
                                <tr>
                                   <td style="width:100%" colspan="4">
                                       <hr />
                                   </td>
                                </tr>
                               </table>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="Tab_Reloj_Checador_Parametros" runat="server" HeaderText="Parámetros Nómina">
                        <HeaderTemplate>Parámetros Reloj Checador</HeaderTemplate>
                        <ContentTemplate>
                            <table width="98%">
                               <tr>
                                   <td style="width:100%" colspan="4"><hr /></td>
                               </tr>
                               <tr>
                                    <td style="text-align:left;width:25%;">*IP del Servidor</td>
                                    <td style="text-align:left;width:75%;" colspan="3">
                                        <asp:TextBox ID="Txt_IP_Servidor_Reloj_Checador" runat="server" TabIndex="8" MaxLength="100" Width="100%"/>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_IP_Servidor_Reloj_Checador" runat="server" 
                                            TargetControlID="Txt_IP_Servidor_Reloj_Checador" ValidChars="./\_- "
                                            FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" />
                                    </td>    
                                </tr>      
                               <tr>
                                    <td style="text-align:left;width:25%;">*Nombre Base de Datos</td>
                                    <td style="text-align:left;width:75%;" colspan="3">
                                        <asp:TextBox ID="Txt_Nombre_Base_Datos_Reloj_Checador" runat="server" TabIndex="8" MaxLength="100" Width="100%"/>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Nombre_Base_Datos_Reloj_Checador" runat="server" 
                                            TargetControlID="Txt_Nombre_Base_Datos_Reloj_Checador" ValidChars="_.- "
                                            FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" />
                                    </td>    
                                </tr>    
                               <tr>
                                    <td style="text-align:left;width:25%;">*Usuario de Base de Datos</td>
                                    <td style="text-align:left;width:25%;">
                                        <asp:TextBox ID="Txt_Usuario_Base_Datos_Reloj_Checador" runat="server" TabIndex="8" MaxLength="50" Width="100%"/>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Usuario_Base_Datos_Reloj_Checador" runat="server" 
                                            TargetControlID="Txt_Usuario_Base_Datos_Reloj_Checador" ValidChars="./\_:;,#@º|$%&/()=-*+?¿!¡ "
                                            FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" />
                                    </td>
                                    <td style="text-align:left;width:25%;">&nbsp;*Password de Base de Datos</td>
                                    <td style="text-align:left;width:25%;">     
                                        <asp:TextBox ID="Txt_Password_Base_Datos_Reloj_Checador" runat="server" TabIndex="13" MaxLength="50" 
                                            Width="100%" TextMode="Password"/>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Password_Base_Datos_Reloj_Checador" runat="server" 
                                            TargetControlID="Txt_Password_Base_Datos_Reloj_Checador" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                            ValidChars="ÑñáéíóúÁÉÍÓÚ./\_:;,#@º|$%&/()=-*+?¿!¡ "/>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


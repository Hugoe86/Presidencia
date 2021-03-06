﻿<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Rpt_Nom_Faltas_Empleados_Manuales.aspx.cs" Inherits="paginas_Nomina_Frm_Ope_Nom_Faltas_Empleados_Manuales" Title="Reporte Faltas Empleados Manuales"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <script type="text/javascript" language="javascript">
        function Limpiar_Ctlr_Campos(){
            document.getElementById("<%=Cmb_Tipos_Nomina.ClientID%>").value="";
            document.getElementById("<%=Cmb_Unidad_Responsable.ClientID%>").value="";
            document.getElementById("<%=Txt_No_Empleado.ClientID%>").value="";
            document.getElementById("<%=Txt_RFC_Empleado.ClientID%>").value="";
            document.getElementById("<%=Txt_Nombre_Empleado.ClientID%>").value="";
            return false;
        }

        function pageLoad(sender, args) {

            $('input[id$=Txt_No_Empleado]').live("blur", function() {
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
    </script> 

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
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"  AsyncPostBackTimeout="36000" EnableScriptLocalization="true" EnableScriptGlobalization="true"/> 
    <asp:UpdatePanel ID="UpPnl_aux_Busqueda" runat="server" UpdateMode="Conditional">
      <ContentTemplate>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpPnl_aux_Busqueda" DisplayAfter="0">
           <ProgressTemplate>
               <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
            </ProgressTemplate>                    
        </asp:UpdateProgress>
   
        <div id="Div_Requisitos" style="background-color:#ffffff; width:100%; height:100%;">
            <table width="99%"  border="0" cellspacing="0" class="estilo_fuente">
                <tr align="center">
                    <td class="label_titulo" colspan="4">Reporte Faltas de Empleados Manuales</td>
                </tr>
                <tr>
                    <td colspan="4">
                      <div id="Div_Contenedor_Msj_Error" style="width:98%;" runat="server" visible="false">
                        <table style="width:100%;">
                          <tr>
                            <td colspan="4" align="left">
                              <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                                Width="24px" Height="24px"/>
                                <asp:Label ID="Lbl_Ecabezado_Mensaje" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                            </td>            
                          </tr>
                          <tr>
                            <td style="width:10%;">              
                            </td>          
                            <td colspan="3">
                              <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                            </td>
                          </tr>          
                        </table>                   
                      </div>                          
                    </td>
                </tr> 
                <tr>
                    <td colspan="2">&nbsp;</td>                        
                </tr>
                <tr class="barra_busqueda" align="right">
                    <td align="left" style="width:50%;">
                        &nbsp;
                        <asp:ImageButton ID="Btn_Generar_Reporte_PDF" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png" Width="24px" ToolTip="Listado para Imprimir" AlternateText="Listado para Imprimir" OnClick="Btn_Generar_Reporte_PDF_Click"/>
                        <asp:ImageButton ID="Btn_Generar_Reporte_Excel" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png" Width="24px" ToolTip="Listado a Excel" AlternateText="Listado a Excel" OnClick="Btn_Generar_Reporte_Excel_Click"/>
                        &nbsp;
                    </td>
                    <td align="right" style="width:50%;">
                        &nbsp;
                        <asp:ImageButton ID="Btn_Limpiar_Campos" runat="server" ToolTip="Limpiar" AlternateText="Limpiar" ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png" Width="24px"  OnClientClick="javascript:return Limpiar_Ctlr_Campos();" />
                        &nbsp;
                    </td>                                      
            </table>  
            <br />  
        </div>
    </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate> 
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
               <ProgressTemplate>
                   <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>                    
            </asp:UpdateProgress>
            <div id="Div_Cuerpo" style="background-color:#ffffff; width:100%; height:100%;">
                <br />  
                <table width="99%" class="estilo_fuente">  
                    <tr>
                        <td style="text-align:center; width:100%" >
                            <asp:Panel ID="Pnl_Listado_Filtros" runat="server" GroupingText="Filtros para Reporte" Font-Bold="true">
                                <table class="estilo_fuente" width="99%">  
                                    <tr>
                                        <td style="text-align:left; width:20%">
                                            <asp:Label ID="Lbl_Tipo_Nomina" runat="server" Text="Tipo Nomina"></asp:Label>
                                        </td>
                                        <td style="text-align:left;" colspan="3">
                                            <asp:DropDownList ID="Cmb_Tipos_Nomina" runat="server" Width="100%">
                                            </asp:DropDownList>
                                        </td>
                                    </tr> 
                                    <tr>
                                        <td style="text-align:left; width:20%">
                                            <asp:Label ID="Lbl_Unidad_Responsable" runat="server" Text="Unidad Responsable"></asp:Label>
                                        </td>
                                        <td style="text-align:left;" colspan="3">
                                            <asp:DropDownList ID="Cmb_Unidad_Responsable" runat="server" Width="100%">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:left; width:20%">
                                            <asp:Label ID="Lbl_No_Empleado" runat="server" Text="No. Empleado"></asp:Label>
                                        </td>
                                        <td style="text-align:left; width:30%">
                                            <asp:TextBox ID="Txt_No_Empleado" runat="server" Width="100%" MaxLength="6"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_No_Empleado" runat="server" TargetControlID="Txt_No_Empleado" FilterType="Numbers" >
                                            </cc1:FilteredTextBoxExtender>
                                            <cc1:TextBoxWatermarkExtender ID="TBW_Txt_No_Empleado" runat="server" TargetControlID="Txt_No_Empleado"
                                            WatermarkCssClass="watermarked2" WatermarkText="No Empleado"/>     
                                        </td>
                                        <td style="text-align:left; width:20%">
                                            &nbsp;
                                            <asp:Label ID="Lbl_RFC_Empleado" runat="server" Text="RFC Empleado"></asp:Label>
                                        </td>
                                        <td style="text-align:left; width:30%">
                                            <asp:TextBox ID="Txt_RFC_Empleado" runat="server" Width="100%" onkeyup='this.value = this.value.toUpperCase();' MaxLength="13"
                                                onblur="$('input[id$=Txt_RFC_Empleado]').filter(function(){if(!this.value.match(/^[a-zA-Z]{3,4}(\d{6})((\D|\d){3})?$/))$(this).val('');});"/>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_RFC_Empleado" runat="server" TargetControlID="Txt_RFC_Empleado"
                                                FilterType="Numbers, UppercaseLetters, LowercaseLetters" ValidChars=" " Enabled="True"/>
                                            <cc1:TextBoxWatermarkExtender ID="TBW_Txt_RFC_Empleado" runat="server" TargetControlID="Txt_RFC_Empleado"
                                            WatermarkCssClass="watermarked2" WatermarkText="RFC"/>     
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:left; width:20%">
                                            <asp:Label ID="Lbl_Nombre_Empleado" runat="server" Text="Nombre Empleado"></asp:Label>
                                        </td>
                                        <td style="text-align:left;" colspan="3">
                                            <asp:TextBox ID="Txt_Nombre_Empleado" runat="server" onkeyup='this.value = this.value.toUpperCase();' Width="100%"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Nombre_Empleado" runat="server" TargetControlID="Txt_Nombre_Empleado" InvalidChars="<,>,&,',!," FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                                            </cc1:FilteredTextBoxExtender> 
                                            <cc1:TextBoxWatermarkExtender ID="TBW_Txt_Empleados" runat="server" TargetControlID="Txt_Nombre_Empleado"
                                                WatermarkCssClass="watermarked2" WatermarkText="Nombre Empleado"/>    
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:left; width:20%">
                                            <asp:Label ID="Lbl_Anio" runat="server" Text="Año"></asp:Label>
                                        </td>
                                        <td style="text-align:left; width:30%">
                                            <asp:DropDownList ID="Cmb_Calendario_Nomina" runat="server" Width="100%" AutoPostBack="true"></asp:DropDownList>
                                        </td>
                                        <td style="text-align:left; width:20%">
                                            &nbsp;
                                            <asp:Label ID="Lbl_Periodo" runat="server" Text="Periodo"></asp:Label>
                                        </td>
                                        <td style="text-align:left; width:30%">
                                            <asp:DropDownList ID="Cmb_Periodos_Catorcenales_Nomina" runat="server" Width="100%" AutoPostBack="true"></asp:DropDownList>
                                        </td>
                                    </tr>  
                                </table> 
                            </asp:Panel>
                        </td>
                        <td align="right" style="width:80%;">&nbsp;</td> 
                    </tr>
                </table>     
            </div>
            <br />
            <br />
            <br />
            <br />
        </ContentTemplate>   
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Btn_Generar_Reporte_PDF" EventName="Click" />            
            <asp:PostBackTrigger ControlID="Btn_Generar_Reporte_Excel" />
            <asp:PostBackTrigger ControlID="Btn_Limpiar_Campos" />         
        </Triggers>        
    </asp:UpdatePanel>  
    
</asp:Content>
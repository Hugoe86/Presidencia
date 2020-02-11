<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Rpt_Nom_Aportacion.aspx.cs" Inherits="paginas_Nomina_Frm_Rpt_Nom_Aportacion" Title="Reporte de Aportación" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript" language="javascript">
        function Limpiar_Ctlr_Campos(){
            document.getElementById("<%=Cmb_Tipos_Nomina.ClientID%>").value="";
            document.getElementById("<%=Cmb_Unidad_Responsable.ClientID%>").value="";
            document.getElementById("<%=Txt_No_Empleado.ClientID%>").value="";
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
<asp:ScriptManager ID="Sm_Rpt_Nom_Aportacion" runat="server" EnableScriptGlobalization="true"
        EnableScriptLocalization="True"/>
<asp:UpdatePanel ID="Upnl_Rpt_Nom_Aportacion" runat="server" UpdateMode="Conditional">
    <ContentTemplate>    
        <asp:UpdateProgress ID="Uprs_Rpt_Nom_Aportacion" runat="server" AssociatedUpdatePanelID="Upnl_Rpt_Nom_Aportacion"
                DisplayAfter="0">
            <ProgressTemplate>
                <div id="Div_Fondo" class="progressBackgroundFilter"></div>
                <div id="Div_Imagen" style="background-color:Transparent; position:fixed; z-index:1001; top:30%; left:43%;">
                    <img src="../imagenes/paginas/Updating.gif"  alt=""/>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    
        <div style="width:98%;background-color:White;">
        
        
            <table style="width:99%;">
                <tr>
                    <td style="width:100%;" align="center">
                        <div id="Contenedor_Titulo" style="color:White;font-size:12;font-weight:bold;border-style:outset;background:url(../imagenes/paginas/titleBackground.png) repeat-x top;background-color:Silver;">
                            <table width="100%">
                                <tr>
                                    <td></td>
                                </tr>            
                                <tr>
                                    <td width="100%">
                                        <font style="color: Black; font-weight: bold;">Reporte de Aportación</font>
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
             
            <table width="98%">           
                <tr>
                    <td>
                        <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false"  />&nbsp;
                        <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"/>
                    </td>
                </tr>
            </table>
        
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
                        <cc1:TextBoxWatermarkExtender ID="TBW_Txt_No_Empleado" runat="server" TargetControlID="Txt_No_Empleado"
                            WatermarkCssClass="watermarked2" WatermarkText="No Empleado"/>
                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_No_Empleado" runat="server" TargetControlID="Txt_No_Empleado" FilterType="Numbers" >
                        </cc1:FilteredTextBoxExtender>     
                    </td>
                    <td style="text-align:left; width:20%">
                        &nbsp;

                    </td>
                    <td style="text-align:left; width:30%">

                    </td>
                </tr>
                <tr>
                    <td style="text-align:left; width:20%">
                        <asp:Label ID="Lbl_Nombre_Empleado" runat="server" Text="Nombre Empleado"></asp:Label>
                    </td>
                    <td style="text-align:left;" colspan="3">
                        <asp:TextBox ID="Txt_Nombre_Empleado" runat="server" Width="100%"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="TBW_Txt_Nombre_Empleado" runat="server" TargetControlID="Txt_Nombre_Empleado"
                            WatermarkCssClass="watermarked2" WatermarkText="Nombre del Empleado"/>
                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Nombre_Empleado" runat="server" TargetControlID="Txt_Nombre_Empleado" InvalidChars="<,>,&,',!," FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                        </cc1:FilteredTextBoxExtender>     
                    </td>
                </tr>
            </table>
        </div>
    </ContentTemplate>
            <Triggers>            
            <asp:PostBackTrigger ControlID="Btn_Generar_Reporte_Excel" />
            <asp:PostBackTrigger ControlID="Btn_Generar_Reporte_Word" />
        </Triggers>
</asp:UpdatePanel>
<table style="width: 98%;">
        <tr>
            <td class="button_autorizar" style="width: 100%; text-align: right; cursor: default;"
                colspan="4">
                <asp:UpdatePanel ID="Upnl_Export_EXCEL" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                    <ContentTemplate>
                        <asp:ImageButton ID="Btn_Generar_Reporte_Excel" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png"
                            OnClick="Btn_Generar_Reporte_Excel_Click" ToolTip="Generar Reporte en EXCEL"
                            Width="32px" Height="32px" Style="cursor: hand;" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="Upnl_Export_WORD" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                    <ContentTemplate>
                        <asp:ImageButton ID="Btn_Generar_Reporte_Word" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_rep_word.png"
                            OnClick="Btn_Generar_Reporte_Word_Click" ToolTip="Generar Reporte Catálogo Empleados en WORD"
                            Width="32px" Height="32px" Style="cursor: hand;" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td style="width: 100%; text-align: left; cursor: default;" colspan="4">
                <hr />
            </td>
        </tr>
</asp:Content>
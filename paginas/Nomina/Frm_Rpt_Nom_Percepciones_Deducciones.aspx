<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Rpt_Nom_Percepciones_Deducciones.aspx.cs" Inherits="paginas_Nomina_Frm_Rpt_Nom_Percepciones_Deducciones" Title="Untitled Page" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
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
<cc1:ToolkitScriptManager ID="Tsm_Rpt_Percepciones_Deduccciones" runat="server"  AsyncPostBackTimeout="360000"/>
    <asp:UpdatePanel ID="UPnl_Rpt_Percepciones_Deduccciones" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="UPnl_Rpt_Percepciones_Deduccciones" DisplayAfter="0">
            <ProgressTemplate>
                <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        
          <div id="Div_Contenedor_Msj_Error" style="width:70%;font-size:9px;text-align:left;" runat="server" visible="false" >
            <table style="width:100%;">
              <tr>
                <td colspan="2" align="left" style="font-size:12px;color:Red;font-family:Tahoma;text-align:left;">
                  <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                    Width="24px" Height="24px"/>
                    Es Necesario Introducir:
                </td>            
              </tr>
              <tr>
                <td style="width:10%;">              
                </td>            
                <td style="font-size:9px;width:90%;text-align:left;" valign="top">
                  <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text=""  CssClass="estilo_fuente_mensaje_error"/>
                </td>
              </tr>          
            </table>                   
            <br />
          </div>  
        
        <div style="width:98%; background-color:White;">
          <table width="100%" title="Control_Errores"> 
                <tr>
                    <td class="button_autorizar" style="width:100%; text-align:center; cursor:default; font-size:12px;">
                        Reporte Percepciones y/o Deducciones
                    </td>               
                </tr>            
                <tr>
                    <td class="button_autorizar"  style="width:100%; text-align:left; cursor:default;">
                    </td>               
                </tr>   
          </table>         
          <table width="100%">
           <tr>
                <td style="width:100%" colspan="4">
                    <hr />
                </td>
            </tr>   
            <tr>
                <td style="width:20%;text-align:left;font-size:11px;">
                   Clave
                </td>
                <td style="width:30%;text-align:left;font-size:11px;">
                   <asp:TextBox ID="Txt_Clave_Percepcion_Deduccion_Busqueda" runat="server" Width="98%" 
                        onkeyup='this.value = this.value.toUpperCase();' MaxLength="20"/>
                   <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Clave_Percepcion_Deduccion_Busqueda" runat="server" 
                        TargetControlID ="Txt_Clave_Percepcion_Deduccion_Busqueda" WatermarkText="< Clave Ej. P001 >" 
                        WatermarkCssClass="watermarked"/>
                </td> 
                <td style="width:20%;text-align:left;font-size:11px;">
                </td>
                <td style="width:30%;text-align:left;font-size:11px;">
                </td>
            </tr>
           <tr>
                <td style="width:100%" colspan="4">
                    <hr />
                </td>
            </tr> 
            <tr>
                <td style="width:20%;text-align:left;font-size:11px;">
                    Tipo
                </td>
                <td style="width:30%;text-align:left;font-size:11px;">
                    <asp:DropDownList ID="Cmb_Tipo_Busqueda" runat="server" Width="100%">
                        <asp:ListItem Value="">&lt;- Seleccione -&gt;</asp:ListItem>
                        <asp:ListItem>PERCEPCION</asp:ListItem>
                        <asp:ListItem>DEDUCCION</asp:ListItem>
                    </asp:DropDownList>
                </td> 
                <td style="width:20%;text-align:left;font-size:11px;">
                    Estatus
                </td>
                <td style="width:30%;text-align:left;">
                    <asp:DropDownList ID="Cmb_Estatus_Busqueda" runat="server" Width="100%">
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
                    <asp:TextBox ID="Txt_Nombre_Busqueda" runat="server" Width="99.5%" 
                        onkeyup='this.value = this.value.toUpperCase();'/>
                   <cc1:TextBoxWatermarkExtender ID="Twm_Txt_Nombre_Busqueda" runat="server" 
                        TargetControlID ="Txt_Nombre_Busqueda" WatermarkText="< Nombre >" 
                        WatermarkCssClass="watermarked"/>
                </td>
            </tr>
            <tr>
                <td style="width:20%;text-align:left;font-size:11px;">
                    Aplica
                </td>
                <td style="width:30%;text-align:left;font-size:11px;">
                    <asp:DropDownList ID="Cmb_Aplica_Busqueda" runat="server" Width="100%">
                        <asp:ListItem Value="">&lt;- Seleccione -&gt;</asp:ListItem>
                        <asp:ListItem>FIJA</asp:ListItem>
                        <asp:ListItem>VARIABLE</asp:ListItem>
                        <asp:ListItem>OPERACION</asp:ListItem>
                    </asp:DropDownList>
                </td> 
                <td style="width:20%;text-align:left;font-size:11px;">
                    Concepto
                </td>
                <td style="width:30%;text-align:left;font-size:11px;">
                    <asp:DropDownList ID="Cmb_Concepto_Busqueda" runat="server" Width="100%">
                        <asp:ListItem Value="">&lt;- Seleccione -&gt;</asp:ListItem>
                        <asp:ListItem>TIPO_NOMINA</asp:ListItem>
                        <asp:ListItem>SINDICATO</asp:ListItem>
                    </asp:DropDownList>
                </td> 
            </tr>
           <tr>
                <td style="width:100%" colspan="4">
                    <hr />
                </td>
            </tr> 
          </table>
        </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Btn_Generar_Reporte" EventName="Click" />
            <asp:PostBackTrigger  ControlID="Btn_Generar_Reporte_Minimo"/>
        </Triggers>
    </asp:UpdatePanel>
    
<table style="width:98%;">       
    <tr>
        <td class="button_autorizar" style="width:100%; text-align:right; cursor:default;" colspan="4">
            <asp:UpdatePanel ID="Upnl_Export_PDF" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                <ContentTemplate>
                   <asp:ImageButton ID="Btn_Generar_Reporte" runat="server"  ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png" 
                    CausesValidation="false" OnClick="Btn_Generar_Reporte_Click" Width="32px" Height="32px"  style="cursor:hand;"
                    ToolTip="Generar Reporte en PDF"/> 
                </ContentTemplate>
            </asp:UpdatePanel>
            
            <asp:UpdatePanel ID="Upnl_Export_EXCEL" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                <ContentTemplate>
                    <asp:ImageButton ID="Btn_Generar_Reporte_Minimo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png" 
                         OnClick="Btn_Generar_Reporte_Minimo_Click" ToolTip="Generar Reporte en EXCEL" Width="32px" Height="32px" style="cursor:hand;"/>
                 </ContentTemplate>
            </asp:UpdatePanel>
        </td>                
    </tr>   
    <tr>
        <td style="width:100%; text-align:left; cursor:default;" colspan="4">
            <hr />
        </td>                
    </tr>                                                                         
</table>    
</asp:Content>


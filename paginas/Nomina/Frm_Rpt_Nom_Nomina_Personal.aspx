<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" 
CodeFile="Frm_Rpt_Nom_Nomina_Personal.aspx.cs" Inherits="paginas_Nomina_Frm_Rpt_Nom_Nomina_Personal" Title="Reporte de Nómina" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
 <script type="text/javascript">      
    function pageLoad() {                
       $('input[id$=Txt_No_Empleado]').live("blur", function(){
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
    }    
    function isNumber(n) {   return !isNaN(parseFloat(n)) && isFinite(n); }      
 </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<asp:ScriptManager ID="Sm_Rpt_Nomina_Empleados" runat="server"/>
<asp:UpdatePanel ID="UPnl_Rpt_Nomina_Empleados" runat="server">
    <ContentTemplate>
    
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="UPnl_Rpt_Nomina_Empleados" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            
            <div id="Div_Rpt_Nomina_Empleados" style="background-color:White; width:98%; height:100%;">
                <table style="width:100%;">
                    <tr>
                        <td style="width:100%;" align="center">
                            <div id="Contenedor_Titulo" style="color:White;font-size:12;font-weight:bold;border-style:outset;background:url(../imagenes/paginas/titleBackground.png) repeat-x top;background-color:Silver;">
                                <table width="100%">
                                    <tr>
                                        <td></td>
                                    </tr>            
                                    <tr>
                                        <td width="100%">
                                            <font style="color: Black; font-weight: bold;">Recibo del Empleado</font>
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
                        <td style="text-align:left; width:20%;">
                            *No Empleado
                        </td>
                        <td style="text-align:left; width:30%;">
                            <asp:TextBox ID="Txt_No_Empleado" runat="server" Width="98%"/>

                        </td>
                        <td style="text-align:left; width:20%;">
                        </td>
                        <td style="text-align:left; width:30%;">
                        </td>
                    </tr>
                </table>
                
                <asp:Panel ID="Pnl_Calendario_Nomina" runat="server" Width="100%">
                    <table width="100%">
                        <tr>
                            <td style="width:20%;text-align:left;">
                                *Nomina
                            </td>
                            <td style="width:30%;text-align:left;">
                                <asp:DropDownList ID="Cmb_Calendario_Nomina" runat="server" Width="100%" AutoPostBack="true" 
                                    TabIndex="5" onselectedindexchanged="Cmb_Calendario_Nomina_SelectedIndexChanged"/>
                            </td>             
                            <td style="width:20%;text-align:left; cursor:default;">
                                &nbsp;&nbsp;*Periodo
                            </td>
                            <td style="width:30%;text-align:left; cursor:default;">
                                <asp:DropDownList ID="Cmb_Periodos_Catorcenales_Nomina" runat="server" 
                                    Width="100%" TabIndex="6"  AutoPostBack="true" OnSelectedIndexChanged="Cmb_Periodo_SelectedIndexChanged"/>
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
                    </table>
                </asp:Panel>
                
                <table width="100%">
                    <tr>
                        <td style="width:100%; text-align:left; cursor:default;" colspan="4">
                           <hr />
                        </td>
                    </tr>
                    <tr>
                        <td style="width:100%; text-align:left; cursor:default;" colspan="4">
                            <asp:Button ID="Btn_Generar_Reporte" runat="server" Text="Mostrar Recibo" 
                                Width="100%" style="color:Black; border-style: outset;width:100%;cursor:hand;padding:2px 4px 2px 4px;background:url(../imagenes/paginas/titleBackground.png) repeat-x top;
                                background-color:Silver;" 
                                OnClick="Btn_Generar_Reporte_Click"/>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:100%; text-align:left; cursor:default;" colspan="4">
                            <hr />
                        </td>
                    </tr>
                </table>
            </div>
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>

<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" 
CodeFile="Frm_Rpt_Com_Mensual_Requisiciones.aspx.cs" Inherits="paginas_Compras_Frm_Rpt_Com_Mensual_Requisiciones" 
Title="Reporte de Requisiciones" EnableEventValidation="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScptM_Rpt_Orden_Compra" runat="server" EnableScriptGlobalization="true" EnableScriptLocalization="true" />  
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <%--<asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>                     
            </asp:UpdateProgress>--%>
            <div style="width:100%;">
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo" colspan="2">Reporte de Requisiciones</td>
                    </tr>
                    <tr>
                        <td colspan ="2">
                          <div id="Div_Contenedor_Msj_Error" style="width:98%;" runat="server" visible="false">
                            <table style="width:100%;">
                              <tr>
                                <td colspan="2" align="left">
                                  <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                                    Width="24px" Height="24px" Enabled="false"/>
                                    <asp:Label ID="Lbl_Ecabezado_Mensaje" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                </td>            
                              </tr>
                              <tr>
                                <td style="width:10%;">              
                                </td>          
                                <td style="width:90%;text-align:left;" valign="top">
                                  <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                </td>
                              </tr>          
                            </table>                   
                          </div>                
                        </td>
                    </tr>
                    <tr>
                        <td colspan ="2">&nbsp;</td>                        
                    </tr>
                    <tr class="barra_busqueda" align="right">
                        <td align="left" style="width:50%">
                            <asp:ImageButton ID="Btn_Salir" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" Width="24px" 
                                CssClass="Img_Button" ToolTip="Salir" 
                                onclick="Btn_Salir_Click"/>
                        </td>
                        <td style="width:50%">&nbsp;</td>
                    </tr>
                </table>   
                <br />
                <center>
                    <table width="98%">
                        <tr>
                            <td style="text-align:left; width:15%;">
                                Tipo de reporte
                            </td>
                            <td colspan="6" style="text-align:left;" align="left">
                                <asp:DropDownList ID="Cmb_Tipo_Reporte" runat="server" Width="50%"
                                    onselectedindexchanged="Cmb_Tipo_Reporte_SelectedIndexChanged" >
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:15%; text-align:left;">
                                <asp:Label ID="Lbl_Fecha_Inicio" runat="server" Text="Fecha del" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width:18%; text-align:left;">
                                <asp:TextBox ID="Txt_Fecha_Inicio" runat="server" Width="60%"
                                onblur="this.value = (this.value.match(/^(([0-9])|([0-2][0-9])|([3][0-1]))\/(ene|feb|mar|abr|may|jun|jul|ago|sep|oct|nov|dic)\/\d{4}$/))? this.value : '';"></asp:TextBox>
                                <asp:ImageButton ID="Btn_Txt_Fecha_Inicio" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                                <cc1:CalendarExtender ID="CE_Txt_Fecha_Inicio" runat="server" TargetControlID="Txt_Fecha_Inicio" 
                                    PopupButtonID="Btn_Txt_Fecha_Inicio" Format="dd/MMM/yyyy"></cc1:CalendarExtender>
                                    <cc1:MaskedEditExtender ID="MEE_Txt_Fecha_Inicio" Mask="99/LLL/9999" runat="server" MaskType="None" 
                                            UserDateFormat="DayMonthYear" UserTimeFormat="None" Filtered="/" TargetControlID="Txt_Fecha_Inicio" 
                                            Enabled="True" ClearMaskOnLostFocus="false"/>  
                                    <cc1:MaskedEditValidator ID="MEV_MEE_Txt_Fecha_Inicio" runat="server" ControlToValidate="Txt_Fecha_Inicio" ControlExtender="MEE_Txt_Fecha_Inicio" 
                                            EmptyValueMessage="Fecha Requerida" InvalidValueMessage="Fecha no valida" IsValidEmpty="false" 
                                            TooltipMessage="Ingresar Fecha" Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>
                            </td>
                            <td style="width:7%; text-align:left;">&nbsp;
                                <asp:Label ID="Lbl_Fecha_Fin" runat="server" Text="Al " CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width:18%; text-align:left;">
                                <asp:TextBox ID="Txt_Fecha_Fin" runat="server" Width="60%"
                                onblur="this.value = (this.value.match(/^(([0-9])|([0-2][0-9])|([3][0-1]))\/(ene|feb|mar|abr|may|jun|jul|ago|sep|oct|nov|dic)\/\d{4}$/))? this.value : '';"></asp:TextBox>
                                <asp:ImageButton ID="Btn_Txt_Fecha_Fin" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                                <cc1:CalendarExtender ID="CE_Txt_Fecha_Fin" runat="server" TargetControlID="Txt_Fecha_Fin" 
                                    PopupButtonID="Btn_Txt_Fecha_Fin" Format="dd/MMM/yyyy"></cc1:CalendarExtender>
                                    <cc1:MaskedEditExtender ID="MEE_Txt_Fecha_Fin" Mask="99/LLL/9999" runat="server" MaskType="None" 
                                            UserDateFormat="DayMonthYear" UserTimeFormat="None" Filtered="/" TargetControlID="Txt_Fecha_Fin" 
                                            Enabled="True" ClearMaskOnLostFocus="false"/>  
                                    <cc1:MaskedEditValidator ID="MEV_MEE_Txt_Fecha_Fin" runat="server" ControlToValidate="Txt_Fecha_Fin" ControlExtender="MEE_Txt_Fecha_Fin" 
                                            EmptyValueMessage="Fecha Requerida" InvalidValueMessage="Fecha no valida" IsValidEmpty="false" 
                                            TooltipMessage="Ingresar Fecha" Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>
                            </td>
                            <td style="width:10%; text-align:left;">
                                <asp:Label ID="Lbl_Estatus" runat="server" Text="Estatus" CssClass="estilo_fuente" Visible="false"></asp:Label>
                            </td>
                            <td style="width:22%;">
                                <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="98%" Visible="false"></asp:DropDownList>
                            </td>
                            <td style="width:15%;">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </center>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <center>
        <div style="text-align:right; width:97%;">
            <asp:Button id="Btn_Generar" runat="server" Text="Generar" CssClass="button" OnClick="Btn_Generar_Click"/>
        </div>
    </center>
</asp:Content>


<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Rpt_Alm_Cuenta_Publica.aspx.cs" Inherits="paginas_Almacen_Frm_Rpt_Alm_Cuenta_Publica"  Title="Reporte de Cuenta Pública" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">

    <asp:ScriptManager ID="ScptM_Bienes_Muebles" runat="server"  EnableScriptGlobalization ="true" EnableScriptLocalization = "True" />
    
    <asp:UpdatePanel ID="Upd_Panel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
               <ProgressTemplate>
                   <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>                    
            </asp:UpdateProgress> 
            <div id="Div_Requisitos" style="background-color:#ffffff; width:100%; height:100%;">
                <table width="99%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo" colspan="4">Listado de Cuenta Pública</td>
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
                        </td>
                        <td align="right" style="width:50%;">
                            &nbsp;
                        </td> 
                    </tr>                                     
                </table>  
                <br />  
                <table width="99%" class="estilo_fuente">                                                                    
                    <tr>
                        <td style="width:18%; text-align:left; ">
                            <asp:HiddenField ID="Hdf_Empleado_ID" runat="server" />
                            <asp:Label ID="Lbl_Empleado" runat="server" Text="Empleado Resguardo" ></asp:Label>
                        </td>
                        <td style="width:32%; text-align:left; ">
                            <asp:TextBox ID="Txt_Numero_Empleado" runat="server" Width="98%" 
                                style="text-align:right;" AutoPostBack="True" 
                                ontextchanged="Txt_Numero_Empleado_TextChanged"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Numero_Empleado" runat="server" TargetControlID="Txt_Numero_Empleado" FilterType="Numbers"/>
                        </td>
                        <td colspan="2" style="text-align:left;">
                            <asp:TextBox ID="Txt_Nombre_Empleado" runat="server" Width="98%" Enabled="false"></asp:TextBox>          
                        </td>
                    </tr>                                                                   
                    <tr>
                        <td style="width:18%; text-align:left; ">
                            <asp:Label ID="Lbl_Dependencia" runat="server" Text="U. Responsable" ></asp:Label>
                        </td>
                        <td colspan="3" style="text-align:left;">
                            <asp:DropDownList ID="Cmb_Dependencia" runat="server" Width="100%">
                                <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>   
                            </asp:DropDownList>                                   
                        </td>
                    </tr>     
                    <tr>
                        <td style="width:18%; text-align:left;">
                            <asp:Label ID="Lbl_Fecha_Inicial" runat="server" Text="Fecha Inicial" ></asp:Label>
                        </td>
                        <td style="width:32%; text-align:left;">
                            <asp:TextBox ID="Txt_Fecha_Inicial" runat="server" Width="80%" MaxLength="20" AutoPostBack="true" CausesValidation="true"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Fecha_Inicial" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                            <cc1:CalendarExtender ID="CE_Txt_Fecha_Inicial" runat="server" TargetControlID="Txt_Fecha_Inicial" PopupButtonID="Btn_Fecha_Inicial" Format="dd/MMM/yyyy">
                            </cc1:CalendarExtender>
                            <cc1:FilteredTextBoxExtender ID="Txt_Fecha_Inicial_FilteredTextBoxExtender" runat="server" TargetControlID="Txt_Fecha_Inicial" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" ValidChars="/_"/>
                            <cc1:MaskedEditExtender ID="Mee_Txt_Fecha_Inicial" Mask="99/LLL/9999" runat="server" MaskType="None" UserDateFormat="DayMonthYear" UserTimeFormat="None" Filtered="/" TargetControlID="Txt_Fecha_Inicial" Enabled="True" ClearMaskOnLostFocus="false"/>
                            <cc1:MaskedEditValidator  
                            ID="Mev_Mee_Txt_Fecha_Inicial" 
                            runat="server" 
                            ControlToValidate="Txt_Fecha_Inicial"
                            ControlExtender="Mee_Txt_Fecha_Inicial" 
                            EmptyValueMessage="La Fecha Inicial es obligatoria"
                             InvalidValueMessage="Fecha Inicial Invalida" 
                            IsValidEmpty="true" 
                            TooltipMessage="Ingrese o Seleccione la Fecha Inicial"
                            Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>  
                        </td>
                        <td style="width:18%; text-align:left;">
                            <asp:Label ID="Lbl_Fecha_Final" runat="server" Text="Fecha Final" ></asp:Label>
                        </td>
                        <td style="width:32%; text-align:left;">
                            <asp:TextBox ID="Txt_Fecha_Final" runat="server" Width="80%" MaxLength="20" AutoPostBack="true"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Fecha_Final" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                            <cc1:CalendarExtender ID="CE_Txt_Fecha_Final" runat="server" TargetControlID="Txt_Fecha_Final" PopupButtonID="Btn_Fecha_Final" Format="dd/MMM/yyyy">
                            </cc1:CalendarExtender>
                            <cc1:FilteredTextBoxExtender ID="Txt_Fecha_Final_FilteredTextBoxExtender" runat="server" TargetControlID="Txt_Fecha_Final" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" ValidChars="/_"/>
                            <cc1:MaskedEditExtender ID="Mee_Txt_Fecha_Final" Mask="99/LLL/9999" runat="server" MaskType="None" UserDateFormat="DayMonthYear" UserTimeFormat="None" Filtered="/" TargetControlID="Txt_Fecha_Final" Enabled="True" ClearMaskOnLostFocus="false"/>
                            <cc1:MaskedEditValidator  
                            ID="Mev_Txt_Fecha_Final" 
                            runat="server" 
                            ControlToValidate="Txt_Fecha_Final"
                            ControlExtender="Mee_Txt_Fecha_Final" 
                            EmptyValueMessage="La Fecha Final es obligatoria"
                            InvalidValueMessage="Fecha Final Invalida" 
                            IsValidEmpty="true" 
                            TooltipMessage="Ingrese o Seleccione la Fecha Inicial"
                            Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>  
                        </td>
                    </tr>                                
                </table>
            </div>    
        </ContentTemplate>        
    </asp:UpdatePanel>  
    <br />
    <table width="99%"  border="0" cellspacing="0" class="estilo_fuente">
        <tr align="center">
            <td style="text-align:right;">
                <asp:Button ID="Btn_Descargar_Excel" runat="server" Text="Exportar Reporte" onclick="Btn_Descargar_Excel_Click" style="border-style:outset; background-color:White; width:200px; height:50px; font-weight:bolder; font-size:medium; cursor:hand;" />   
                &nbsp;&nbsp;&nbsp;  
            </td>
        </tr>
    </table>
    <br />
    <br />
    <br />

</asp:Content>


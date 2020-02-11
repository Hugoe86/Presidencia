<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" 
CodeFile="Frm_Rpt_Nom_Plantillas_Constancias.aspx.cs" Inherits="paginas_Nomina_Frm_Rpt_Nom_Plantillas_Constancias" Title="Untitled Page" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<asp:ScriptManager ID="Sm_Reporte" runat="server"/>
<asp:UpdatePanel ID="UPnl_Reporte" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    
        <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="UPnl_Reporte" DisplayAfter="0">
            <ProgressTemplate>
                <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
            </ProgressTemplate>
        </asp:UpdateProgress>  
         
        <div style="width:98%; background-color:White;">
        
            <table width="100%" title="Control_Errores"> 
                <tr>
                    <td class="button_autorizar" style="width:100%; text-align:center; cursor:default; font-size:12px;">
                        Constancias
                    </td>
                </tr>
                <tr>
                    <td class="button_autorizar"  style="width:100%; text-align:left; cursor:default;">
                        <div id="Div_Contenedor_Msj_Error" runat="server">
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" 
                            style="display:none;" Width="24px" Height="24px"/>
                            <asp:Label id="Lbl_Mensaje_Error" runat="server" CssClass="estilo_fuente_mensaje_error" />
                        </div>
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td class="button_autorizar" style="width:20%; text-align:left; cursor:default;">
                        Tipo Constancia
                    </td>
                    <td class="button_autorizar" style="text-align:left; cursor:default;" colspan="3">
                        <asp:RadioButtonList ID="Rbl_Reporte" runat="server" >
                            <asp:ListItem Text="Constancia Registro Patronal" Value="0" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Constancia" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Constancia Trabajo" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Constancia Baja" Value="3"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td> 
                </tr>
                <tr>
                    <td class="button_autorizar" style="text-align:left; cursor:default;" colspan="4">
                        Datos
                    </td>
                </tr>
                <tr>
                    <td class="button_autorizar" style="width:20%; text-align:left; cursor:default;">
                        Asunto Constancia
                    </td>
                    <td class="button_autorizar" style="text-align:left; cursor:default;" colspan="3">
                        <asp:TextBox ID="Txt_Titulo" runat="server" Width="100%" MaxLength="150"/>
                    </td> 
                </tr>
                <tr>
                    <td class="button_autorizar" style="width:20%; text-align:left; cursor:default;">
                        Presente
                    </td>
                    <td class="button_autorizar" style="text-align:left; cursor:default;" colspan="3">
                        <asp:TextBox ID="Txt_Presente" runat="server" Width="100%" MaxLength="150"/>
                    </td> 
                </tr>
                <tr>
                    <td class="button_autorizar" style="width:20%; text-align:left; cursor:default;">
                        Empleado
                    </td>
                    <td class="button_autorizar" style="width:30%; text-align:left; cursor:default;">
                        <asp:TextBox ID="Txt_No_Empleado" runat="server" Width="200px" MaxLength="6"/>
                        <cc1:TextBoxWatermarkExtender ID="TBW_Txt_Empleados" runat="server" TargetControlID="Txt_No_Empleado"
                            WatermarkCssClass="watermarked2" WatermarkText="No Empleado"/>
                        <cc1:FilteredTextBoxExtender ID="FTxt_No_Empleado" 
                             runat="server" TargetControlID="Txt_No_Empleado" FilterType="Numbers"/>   
                    </td> 
                    <td class="button_autorizar" style="width:50%; text-align:left; cursor:default;" colspan="2">
                        <asp:TextBox ID="Txt_Nombre_Empleado" runat="server" Width="99%" MaxLength="100"/>
                        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="Txt_Nombre_Empleado"
                            WatermarkCssClass="watermarked2" WatermarkText="Nombre Empleado"/>
                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" 
                             runat="server" TargetControlID="Txt_Nombre_Empleado" FilterType="Custom,LowercaseLetters, UppercaseLetters"
                             ValidChars=",ÓÚÍÁÉóúíéá. ü"/>   
                    </td> 
                </tr>
                 <tr>
                    <td class="button_autorizar" style="width:20%; text-align:left; cursor:default;">
                        Datos Dependencia
                    </td>
                    <td class="button_autorizar" style="text-align:left; cursor:default;" colspan="3">
                        <asp:TextBox ID="Txt_Datos_UR" runat="server" Width="100%" MaxLength="250"/>
                        <cc1:TextBoxWatermarkExtender ID="TBW_UR" runat="server" TargetControlID="Txt_Datos_UR"
                            WatermarkCssClass="watermarked2" WatermarkText="Teléfono, Ubicación"/>
                    </td> 
                </tr>
                <tr>
                    <td class="button_autorizar" style="width:20%; text-align:left; cursor:default;">
                        Puesto Empleado
                    </td>
                    <td class="button_autorizar" style="text-align:left; cursor:default;" colspan="3">
                        <asp:TextBox ID="Txt_Puesto" runat="server" Width="100%" MaxLength="150"/>
                    </td> 
                </tr>
                <tr>
                    <td class="button_autorizar" style="width:20%; text-align:left; cursor:default;">
                        Director RH
                    </td>
                    <td class="button_autorizar" style="text-align:left; cursor:default;" colspan="3">
                        <asp:TextBox ID="Txt_Director" runat="server" Width="100%" MaxLength="150" Text="RAMÓN RAMOS SAAVEDRA"/>
                    </td> 
                </tr>
            </table> 
            <table width="100%">
                <tr>
                    <td style="width:100%; text-align:left; cursor:default;" colspan="4">
                        <hr />
                    </td>
                </tr>
            </table>
        </div>
    </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Btn_Generar_Reporte" EventName="Click" />
        </Triggers>    
</asp:UpdatePanel>

<table style="width:98%;">       
    <tr>
        <td class="button_autorizar" style="width:100%; text-align:right; cursor:default;" colspan="4">
            <asp:UpdatePanel ID="Upnl_Export_PDF" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                <ContentTemplate>
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Upnl_Export_PDF" DisplayAfter="0">
                        <ProgressTemplate>
                            <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                            <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>  
                   <asp:ImageButton ID="Btn_Generar_Reporte" runat="server"  ImageUrl="~/paginas/imagenes/paginas/icono_rep_word.png" 
                    CausesValidation="false" OnClick="Btn_Generar_Reporte_Click" Width="32px" Height="32px"  style="cursor:hand;"
                    ToolTip="Generar Reporte en PDF"/> 
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


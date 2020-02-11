<%@ Page Title="Interfaces Bancos" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Nom_Bancos.aspx.cs" Inherits="paginas_Nomina_Frm_Ope_Nom_Bancos" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">

    <asp:ScriptManager ID="SM_Nomina_Bancos" runat="server" />
    <asp:UpdatePanel ID="UPnl_Nomina_Bancos" runat="server" UpdateMode="Always" >
        <ContentTemplate>

            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="UPnl_Nomina_Bancos" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress">
                        <img alt="" src="../Imagenes/paginas/Updating.gif" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>       
            
            <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                <tr>
                    <td>
                        <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false"  />&nbsp;
                        <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"/>
                    </td>
                </tr>
            </table>       
            
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
                                        <font style="color: Black; font-weight: bold;">Generar Archivos Bancos</font>
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

            <table width="99%">
                <tr>
                    <td class="button_autorizar"  style="width:98%;text-align:left;" colspan="4">                        
                       &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="width:100%">
                        <div id="Div_Periodo_Pago"  style="font-size:12;color:White; font-weight:bold;border-style:outset; cursor:auto;width:99%;">
                            <table width="100%">
                                <tr>
                                    <td class="button_autorizar"  style="width:20%;text-align:left;color:Black;">
                                        *Nomina
                                    </td>
                                    <td class="button_autorizar"  style="width:30%;text-align:left;">
                                        <asp:DropDownList ID="Cmb_Calendario_Nomina" runat="server" Width="100%" AutoPostBack="true" 
                                            onselectedindexchanged="Cmb_Calendario_Nomina_SelectedIndexChanged"/>
                                    </td>             
                                    <td class="button_autorizar"  style="width:20%;text-align:left;color:Black;">
                                        *Periodo
                                    </td>
                                    <td class="button_autorizar"  style="width:30%;text-align:left;">
                                        <asp:DropDownList ID="Cmb_Periodos_Catorcenales_Nomina" runat="server" 
                                            Width="100%"  AutoPostBack="true" OnSelectedIndexChanged="Cmb_Periodos_Catorcenales_Nomina_SelectedIndexChanged"/>
                                    </td>                                                                                        
                                </tr>
                            </table>
                        </div>                  
                    </td>
                </tr> 
                <tr>
                    <td class="button_autorizar" style="width:20%;">
                        Fecha Inicial
                    </td>
                    <td class="button_autorizar" style="width:30%;">
                        <asp:TextBox ID="Txt_Inicia_Catorcena" runat="server" Width="98%" Enabled="false"/>
                    </td>            
                    <td class="button_autorizar" style="width:20%;">
                        Fecha Final
                    </td>
                    <td class="button_autorizar" style="width:30%;">
                        <asp:TextBox ID="Txt_Fin_Catorcena" runat="server" Width="98%" Enabled="false"/>
                    </td>                        
                </tr>
                <tr>
                    <td class="button_autorizar"  style="width:98%;text-align:left;" colspan="4">                        
                       &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="button_autorizar"  style="width:20%;text-align:left;">                        
                       Tipo Nomina
                    </td>
                    <td class="button_autorizar"  style="width:30%;text-align:left;">
                        <asp:DropDownList ID="Cmb_Tipos_Nominas" runat="server" Width="100%"
                            AutoPostBack="true" OnSelectedIndexChanged="Cmb_Tipos_Nominas_SelectedIndexChanged"/>
                    </td>
                    <td class="button_autorizar"  style="width:20%;text-align:left;"> 
                        Banco
                    </td>
                    <td class="button_autorizar"  style="width:30%;text-align:left;">
                        <asp:DropDownList ID="Cmb_Bancos" runat="server" Width="100%" AutoPostBack="true"
                            OnSelectedIndexChanged="Cmb_Bancos_SelectedIndexChanged"/>
                    </td>
                </tr>
                <tr>
                    <td class="button_autorizar"  style="width:98%;text-align:left;" colspan="4">                        
                       &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="button_autorizar"  style="width:20%;text-align:left;">                        
                       Nombre Archivo
                    </td>
                    <td class="button_autorizar"  style="width:30%;text-align:left;" colspan="3">
                        <asp:TextBox ID="Txt_Nombre_Archivo" runat="server" Width="99.5%" Enabled="false"/>
                        <cc1:FilteredTextBoxExtender ID="FTxt_Nombre_Archivo" runat="server"  TargetControlID="Txt_Nombre_Archivo"
                            FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars="áéíóúÁÉÍÓÚ ñÑ._-"/>    
                        <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Nombre_Archivo" runat="server" TargetControlID ="Txt_Nombre_Archivo" 
                            WatermarkText="Nombre del Archivo Generar" WatermarkCssClass="watermarked"/>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <div id="Div1"  style="font-size:12;color:White; font-weight:bold;border-style:outset; cursor:auto;width:98%;">
       <asp:Button ID="Btn_Generar_Archivo" runat="server" Text="Generar Archivo" Width="100%"
        OnClick="Btn_Generar_Archivo_Click"  CssClass="button_autorizar" ToolTip="Consultar"/>
    </div> 
    
</asp:Content>


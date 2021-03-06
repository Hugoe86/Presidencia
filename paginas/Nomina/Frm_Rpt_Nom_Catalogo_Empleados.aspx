﻿<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Rpt_Nom_Catalogo_Empleados.aspx.cs" Inherits="paginas_Nomina_Reporte_Frm_Rpt_Nom_Catalogo_Empleados"
    Title="Reporte Catalogo de Empleados" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">

    <script type="text/javascript" language="javascript">
function pageLoad(){
   $('input[id$=Txt_Busqueda_No_Empleado]').live("blur", function(){
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
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <asp:ScriptManager ID="SM_Rpt_Empleados" runat="server" EnableScriptGlobalization="true"
        EnableScriptLocalization="True" />
    <asp:UpdatePanel ID="UPnl_Rpt_Empleados" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="UPnl_Rpt_Empleados"
                DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                    </div>
                    <div class="processMessage" id="div_progress">
                        <img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div style="background-color: White; width: 98%;">
                <table style="width: 99%;">
                    <tr>
                        <td style="width: 100%;" align="center">
                            <div id="Contenedor_Titulo" style="color: White; font-size: 12; font-weight: bold;
                                border-style: outset; background: url(../imagenes/paginas/titleBackground.png) repeat-x top;
                                background-color: Silver;">
                                <table width="100%">
                                    <tr>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="100%" align="left">
                                            <font style="color: Black; font-weight: bold;">Reporte Catálogo de Empleados</font>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
                <table width="100%" class="estilo_fuente">
                    <tr>
                        <td colspan="4">
                            &nbsp;
                            <asp:UpdatePanel ID="Upnl_Mensajes_Error" runat="server" UpdateMode="Always" RenderMode="Inline">
                                <ContentTemplate>
                                    <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png"
                                        Visible="false" />&nbsp;
                                    <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
                <table style="width: 100%;">
                    <tr>
                        <td class="button_autorizar" style="width: 20%; text-align: left; cursor: default;">
                            No Empleado
                        </td>
                        <td class="button_autorizar" style="width: 30%; text-align: left; cursor: default;">
                            <asp:TextBox ID="Txt_Busqueda_No_Empleado" runat="server" Width="98%" TabIndex="0" />
                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_No_Empleado" runat="server" TargetControlID="Txt_Busqueda_No_Empleado"
                                FilterType="Numbers" />
                        </td>
                        <td class="button_autorizar" style="width: 20%; text-align: left; cursor: default;">
                            Estatus
                        </td>
                        <td class="button_autorizar" style="width: 30%; text-align: left; cursor: default;">
                            <asp:DropDownList ID="Cmb_Busqueda_Estatus" runat="server" Width="100%">
                                <asp:ListItem Value="">&lt;- Seleccione -&gt;</asp:ListItem>
                                <asp:ListItem>ACTIVO</asp:ListItem>
                                <asp:ListItem>INACTIVO</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="button_autorizar" style="width: 20%; text-align: left; cursor: default;">
                            Nombre
                        </td>
                        <td class="button_autorizar" style="width: 80%; text-align: left; cursor: default;"
                            colspan="3">
                            <asp:TextBox ID="Txt_Busqueda_Nombre_Empleado" runat="server" Width="98%" TabIndex="0"
                                onkeyup='this.value = this.value.toUpperCase();' />
                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_Nombre_Empleado" runat="server"
                                TargetControlID="Txt_Busqueda_Nombre_Empleado" FilterType="Custom, UppercaseLetters, LowercaseLetters"
                                ValidChars="ÑñáéíóúÁÉÍÓÚ " />
                        </td>
                    </tr>
                    <tr>
                        <td class="button_autorizar" style="width: 20%; text-align: left; cursor: default;">
                            RFC
                        </td>
                        <td class="button_autorizar" style="width: 30%; text-align: left; cursor: default;">
                            <asp:TextBox ID="Txt_Busqueda_RFC_Empleado" runat="server" Width="98%" TabIndex="0"
                                onkeyup='this.value = this.value.toUpperCase();' />
                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_RFC_Empleado" runat="server" TargetControlID="Txt_Busqueda_RFC_Empleado"
                                ValidChars=" " FilterType="Numbers, UppercaseLetters, LowercaseLetters" />
                        </td>
                        <td class="button_autorizar" style="width: 20%; text-align: left; cursor: default;">
                            CURP
                        </td>
                        <td class="button_autorizar" style="width: 30%; text-align: left; cursor: default;">
                            <asp:TextBox ID="Txt_Busqueda_CURP_Empleado" runat="server" Width="98%" TabIndex="0"
                                onkeyup='this.value = this.value.toUpperCase();' />
                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_CURP_Empleado" runat="server" TargetControlID="Txt_Busqueda_CURP_Empleado"
                                ValidChars=" " FilterType="Numbers, UppercaseLetters, LowercaseLetters" />
                        </td>
                    </tr>
                    <tr>
                        <td class="button_autorizar" style="width: 20%; text-align: left; cursor: default;">
                            Tipo Nómina
                        </td>
                        <td class="button_autorizar" style="width: 30%; text-align: left; cursor: default;">
                            <asp:DropDownList ID="Cmb_Busqueda_Tipo_Nomina" runat="server" Width="100%" />
                        </td>
                        <td class="button_autorizar" style="width: 20%; text-align: left; cursor: default;">
                            Sindicato
                        </td>
                        <td class="button_autorizar" style="width: 30%; text-align: left; cursor: default;">
                            <asp:DropDownList ID="Cmb_Busqueda_Sindicato" runat="server" Width="100%" />
                        </td>
                    </tr>
                    <tr>
                        <td class="button_autorizar" style="width: 20%; text-align: left; cursor: default;">
                            U. Responsable
                        </td>
                        <td class="button_autorizar" style="width: 80%; text-align: left; cursor: default;"
                            colspan="3">
                            <asp:DropDownList ID="Cmb_Busqueda_Unidad_Responsable" runat="server" Width="100%" />
                        </td>
                    </tr>
                    <tr>
                        <td class="button_autorizar" style="width: 20%; text-align: left; cursor: default;">
                            Ordenamiento
                        </td>
                        <td class="button_autorizar" style="width: 80%; text-align: left; cursor: default;"
                            colspan="3">
                            <asp:DropDownList ID="Cmb_Ordenamiento" runat="server" Width="100%" >
                                <asp:ListItem>CATALOGO DE EMPLEADOS</asp:ListItem>
                                <asp:ListItem>CATALOGO DE EMPLEADOS ORDENADO POR NUMERO DE EMPLEADO</asp:ListItem>
                                <asp:ListItem>CATALOGO DE EMPLEADOS ORDENADO POR SINDICATO</asp:ListItem>
                                <asp:ListItem>CATALOGO DE EMPLEADOS ORDENADO POR RFC</asp:ListItem>
                                <asp:ListItem>CATALOGO DE EMPLEADOS ORDENADO POR PUESTO</asp:ListItem>
                                <asp:ListItem>CATALOGO DE EMPLEADOS ORDENADO POR BANCO</asp:ListItem>
                                <asp:ListItem>CATALOGO DE EMPLEADOS ORDENADO POR ANTIGÜEDAD</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%; text-align: left; cursor: default;" colspan="4">
                            <hr />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Btn_Generar_Reporte" EventName="Click" />
            <asp:PostBackTrigger ControlID="Btn_Generar_Reporte_Excel" />
            <asp:PostBackTrigger ControlID="Btn_Generar_Reporte_Word" />
        </Triggers>
    </asp:UpdatePanel>
    <table style="width: 98%;">
        <tr>
            <td class="button_autorizar" style="width: 100%; text-align: right; cursor: default;"
                colspan="4">
                <asp:UpdatePanel ID="Upnl_Export_PDF" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                    <ContentTemplate>
                        <asp:ImageButton ID="Btn_Generar_Reporte" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png"
                            OnClick="Btn_Generar_Reporte_Click" ToolTip="Generar Reporte Catálogo Empleados en PDF"
                            Width="32px" Height="32px" Style="cursor: hand;" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="Upnl_Export_EXCEL" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                    <ContentTemplate>
                        <asp:ImageButton ID="Btn_Generar_Reporte_Excel" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png"
                            OnClick="Btn_Generar_Reporte_Excel_Click" ToolTip="Generar Reporte Catálogo Empleados en EXCEL"
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
    </table>
</asp:Content>

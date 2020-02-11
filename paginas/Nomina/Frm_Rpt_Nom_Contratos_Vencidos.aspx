<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" 
CodeFile="Frm_Rpt_Nom_Contratos_Vencidos.aspx.cs" Inherits="paginas_Nomina_Frm_Rpt_Nom_Contratos_Vencidos" Title="SIAG Sistema Administrativo Gubernamental" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
<script type="text/javascript">
    $(document).ready(function (){
        var fecha = new Date();
        var fechaDia = fecha.getDate();
        var fechaMes = fecha.getMonth() + 1;
        var fechaAnio = fecha.getFullYear();
    
        switch(fechaMes)
        {
            case 1: fechaMes = "ene"; break;
            case 2: fechaMes = "feb"; break;
            case 3: fechaMes = "mar"; break;
            case 4: fechaMes = "abr"; break;
            case 5: fechaMes = "may"; break;
            case 6: fechaMes = "jun"; break;
            case 7: fechaMes = "jul"; break;
            case 8: fechaMes = "ago"; break;
            case 9: fechaMes = "sep"; break;
            case 10: fechaMes = "oct"; break;
            case 11: fechaMes = "nov"; break;
            case 12: fechaMes = "dic"; break;
        }
    
        $("select[id$=Cmb_Tipo]").change(function () {
            $("select[id$=Cmb_Tipo] option:selected").each(function () {
                if ($(this).val() == "VENCIDO") {
                    $("input[id$=Txt_Fecha_Fin]").val(fechaDia + "/" + fechaMes + "/" + fechaAnio);
                    $("input[id$=Txt_Fecha_Inicio]").attr("disabled", false);
                    $("input[id$=Txt_Fecha_Fin]").attr("disabled", true);
                }
                else if ($(this).val() == "") {
                    $("input[id$=Txt_Fecha_Inicio]").attr("disabled", false);
                    $("input[id$=Txt_Fecha_Fin]").attr("disabled", false);
                    $("input[id$=Txt_Fecha_Inicio]").val("");
                    $("input[id$=Txt_Fecha_Fin]").val("");
                }
                else {
                    $("input[id$=Txt_Fecha_Inicio]").val(fechaDia + "/" + fechaMes + "/" + fechaAnio);
                    $("input[id$=Txt_Fecha_Inicio]").attr("disabled", true);
                    $("input[id$=Txt_Fecha_Fin]").attr("disabled", false);
                }
            });
        }).trigger('change');
    });
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<asp:ScriptManager ID="ScptM_Rpt_Orden_Compra" runat="server" EnableScriptGlobalization="true" EnableScriptLocalization="true" />  
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>                     
            </asp:UpdateProgress>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div style="width:100%;">
        <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
            <tr><td colspan="2" style="height:0.5em;"></td></tr>
            <tr align="center">
                <td class="label_titulo" colspan="2">Reporte de Contratos Vencidos y Por Vencer</td>
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
            <tr><td colspan="2" style="height:0.5em;"></td></tr>
            <tr class="barra_busqueda" align="right">
                <td align="left" style="width:50%">
                    <asp:ImageButton ID="Btn_Salir" runat="server" 
                        ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" Width="24px" 
                        CssClass="Img_Button" ToolTip="Salir" 
                        OnClick="Btn_Salir_Click"/>
                </td>
                <td style="width:50%">&nbsp;</td>
            </tr>
        </table>   
        <br />
        <center>
          <asp:Panel ID="Panel1" runat="server" GroupingText="Filtros de reporte" Width="96%">
            <center>
                 <table width="98%">
                    <tr>
                        <td style="width:12%; text-align:left; cursor:default" class="button_autorizar">
                            <asp:Label ID="Lbl_Empleado" runat="server" Text="Empleado"></asp:Label>
                        </td>
                        <td style="width:17%; text-align:left; cursor:default;" class="button_autorizar">
                            <asp:TextBox ID="Txt_No_Empleado" runat="server" Width="98%" MaxLength="10"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_No_Empleado" runat="server" WatermarkText="<No. Empleado>" TargetControlID="Txt_No_Empleado" WatermarkCssClass="watermarked" />
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_No_Empleado" runat="server" TargetControlID="Txt_No_Empleado" FilterType="Numbers" ></cc1:FilteredTextBoxExtender>
                        </td>
                        <td colspan="4" style="width:60%; text-align:left; vertical-align:middle; cursor:default;" class="button_autorizar">
                            <asp:TextBox ID="Txt_Nombre_Empleado" runat="server" Width="98%" MaxLength="150"/>
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Nombre_Empleado" runat="server" WatermarkText="<Nombre Empleado>" 
                                TargetControlID="Txt_Nombre_Empleado" WatermarkCssClass="watermarked" />
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Nombre_Empleado" runat="server" TargetControlID="Txt_Nombre_Empleado" 
                                FilterType="UppercaseLetters, LowercaseLetters, Custom" 
                                ValidChars="ÑñáéíóúÁÉÍÓÚ. "/> 
                        </td>
                    </tr>
                    
                    <tr>
                        <td style="width:12%; text-align:left; cursor:default;" class="button_autorizar">
                            <asp:Label ID="Lbl_Fecha_Inicio" runat="server" Text="* Fecha del" ></asp:Label>
                        </td>
                        <td style="width:17%; text-align:left; cursor:default;" class="button_autorizar">
                            <asp:TextBox ID="Txt_Fecha_Inicio" runat="server" Width="79%" MaxLength="11" 
                            onblur="this.value = (this.value.match(/^(([0-9])|([0-2][0-9])|([3][0-1]))\/(ene|feb|mar|abr|may|jun|jul|ago|sep|oct|nov|dic)\/\d{4}$/))? this.value : '';"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Txt_Fecha_Inicio" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                            <cc1:CalendarExtender ID="CE_Txt_Fecha_Inicio" runat="server" TargetControlID="Txt_Fecha_Inicio" 
                                PopupButtonID="Btn_Txt_Fecha_Inicio" Format="dd/MMM/yyyy"></cc1:CalendarExtender>
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Fecha_Inicio" runat="server" WatermarkText="<dd/MMM/aaaa>" 
                                TargetControlID="Txt_Fecha_Inicio" WatermarkCssClass="watermarked" />
                        </td>
                        <td style="width:7%; text-align:left; cursor:default;" class="button_autorizar">&nbsp;
                            <asp:Label ID="Lbl_Fecha_Fin" runat="server" Text="* Al "></asp:Label>
                        </td>
                        <td style="width:20%; text-align:left; cursor:default;" class="button_autorizar">
                            <asp:TextBox ID="Txt_Fecha_Fin" runat="server" Width="78%" MaxLength="11"
                            onblur="this.value = (this.value.match(/^(([0-9])|([0-2][0-9])|([3][0-1]))\/(ene|feb|mar|abr|may|jun|jul|ago|sep|oct|nov|dic)\/\d{4}$/))? this.value : '';"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Txt_Fecha_Fin" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                            <cc1:CalendarExtender ID="CE_Txt_Fecha_Fin" runat="server" TargetControlID="Txt_Fecha_Fin" 
                                PopupButtonID="Btn_Txt_Fecha_Fin" Format="dd/MMM/yyyy"></cc1:CalendarExtender>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Fecha_Fin" runat="server" WatermarkText="<dd/MMM/aaaa>" 
                                TargetControlID="Txt_Fecha_Fin" WatermarkCssClass="watermarked" />
                        </td>
                        <td style="width:10%; text-align:left; cursor:default;" class="button_autorizar">
                            <asp:Label ID="Lbl_Tipo" runat="server" Text="&nbsp;&nbsp; Tipo "></asp:Label>
                        </td>
                        <td style="width:25%; text-align:left; cursor:default;" class="button_autorizar">
                            <asp:DropDownList ID="Cmb_Tipo" runat="server" Width="98%"></asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </center>
          </asp:Panel>
          <table width="96%">
            <tr><td class="button_autorizar" style="cursor:default; width:98%;"><hr /></td></tr>
            <tr>
                <td style="cursor:default; width:98%; text-align:right;" class="button_autorizar">
                    <asp:ImageButton ID="IBtn_Generar" runat="server" style="cursor:pointer;"
                            ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png" Width="24px" 
                            CssClass="Img_Button" ToolTip="Generar Reporte" 
                            OnClick="Btn_Generar_Click"/>
                </td>
            </tr>
          </table>
        </center>
    </div>
</asp:Content>


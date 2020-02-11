<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Rpt_Nom_Pago_Terceros.aspx.cs" Inherits="paginas_Nomina_Frm_Rpt_Nom_Pago_Terceros" Title="Untitled Page" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
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
        
            <div style="width:100%;">
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr><td colspan="2" style="height:0.5em;"></td></tr>
                    <tr align="center">
                        <td class="label_titulo" colspan="2">Reporte de Pago a Terceros</td>
                    </tr>
                    <tr>
                        <td colspan="2">
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
                  <asp:Panel runat="server" GroupingText="Filtros de reporte" Width="96%">
                    <center>
                        <table width="97%">
                            <tr><td colspan="6" style="height:0.3em;"></td></tr>
                            <tr>
                                <td style="width:13%; text-align:left; cursor:default;" class="button_autorizar">
                                    <asp:Label ID="Lbl_Empleado" runat="server" Text="Empleado"></asp:Label>
                                </td>
                                <td style="width:25%; text-align:left; cursor:default;" class="button_autorizar">
                                    <asp:TextBox ID="Txt_No_Empleado" runat="server" Width="89%" MaxLength="10"></asp:TextBox>
                                    <cc1:TextBoxWatermarkExtender ID="TWE_Txt_No_Empleado" runat="server" WatermarkText="<No. Empleado>" TargetControlID="Txt_No_Empleado" WatermarkCssClass="watermarked" />
                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_No_Empleado" runat="server" TargetControlID="Txt_No_Empleado" FilterType="Numbers" ></cc1:FilteredTextBoxExtender>
                                </td>
                                <td colspan="4" style="width:62%; text-align:left; vertical-align:middle; cursor:default;" class="button_autorizar">
                                    <asp:TextBox ID="Txt_Nombre_Empleado" runat="server" Width="98%" MaxLength="150"/>
                                        <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Nombre_Empleado" runat="server" WatermarkText="<Nombre Empleado>" 
                                        TargetControlID="Txt_Nombre_Empleado" WatermarkCssClass="watermarked" />
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Nombre_Empleado" runat="server" TargetControlID="Txt_Nombre_Empleado" 
                                        FilterType="UppercaseLetters, LowercaseLetters, Custom" 
                                        ValidChars="ÑñáéíóúÁÉÍÓÚ. "/>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align:left; width:13%; cursor:default;" class="button_autorizar">
                                    <asp:Label ID="Lbl_Nomina" runat="server" Text="* Nomina"></asp:Label>
                                </td>
                                <td style="text-align:left; width:25%; cursor:default" class="button_autorizar">
                                    <asp:DropDownList ID="Cmb_Nomina" runat="server" Width="90%" OnSelectedIndexChanged="Cmb_Nomina_SelectedIndexChanged" AutoPostBack="true" ></asp:DropDownList>
                                </td>
                                <td style="width:15%; text-align:left; cursor:default;" class="button_autorizar">&nbsp;
                                    <asp:Label ID="Lbl_Periodo" runat="server" Text="* Periodo" ></asp:Label>
                                </td>
                                <td style="width:47%; text-align:left; cursor:default;" class="button_autorizar" colspan="3">
                                    <asp:DropDownList ID="Cmb_Periodo" runat="server" Width="55%"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr><td colspan="6" style="height:0.3em;"></td></tr>
                        </table>
                    </center>
                  </asp:Panel>
                </center>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div style="width:100%;">
        <center>
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


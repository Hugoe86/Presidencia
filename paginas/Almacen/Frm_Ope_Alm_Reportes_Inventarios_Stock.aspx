<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Alm_Reportes_Inventarios_Stock.aspx.cs" Inherits="paginas_Almacen_Frm_Ope_Alm_Reportes_Inventarios_Stock" Title="Reporte Inventarios Stock" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <%-- <script type="text/javascript" language="javascript">
                function calendarShown(sender, args){
                    sender._popupBehavior._element.style.zIndex = 10000005;
                }
    </script> --%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
   <asp:ScriptManager ID="ScriptManager_Reportes" runat="server" EnableScriptGlobalization = "true" EnableScriptLocalization = "True" />
       <asp:UpdatePanel ID="Upd_Panel" runat="server" UpdateMode="Always">
          <ContentTemplate>
           <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <%--<div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress">
                        <img alt="" src="../Imagenes/paginas/Updating.gif" />
                    </div>--%>
                </ProgressTemplate>
               </asp:UpdateProgress>
                <div id="Div_Contenido" style="background-color:#ffffff; width:98%; height:100%;"> <%--Fin del div General--%>
                    <table  border="0" cellspacing="0" class="estilo_fuente" frame="border" >
                    <tr>
                       <td class="label_titulo">Reporte de Inventarios de Stock
                       </td>
                     </tr>
                     <tr> <!--Bloque del mensaje de error-->
                        <td>
                           <div id="Div_Contenedor_Msj_Error" style="width:95%;font-size:9px;" runat="server" visible="false">
                              <table style="width:100%;">
                                <tr>
                                    <td align="left" style="font-size:12px;color:Red;font-family:Tahoma;text-align:left;">
                                        <asp:Image ID="Img_Warning" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                                        Width="24px" Height="24px" />
                                    </td>            
                                    <td style="font-size:9px;width:90%;text-align:left;" valign="top">
                                        <asp:Label ID="Lbl_Informacion" runat="server" ForeColor="Red" />
                                    </td>
                                </tr> 
                            </table>                   
                          </div>
                        </td>
                     </tr>
                     
                     <tr class="barra_busqueda">
                        <td style="width:20%;">
                           
                             <asp:ImageButton ID="Btn_Imprimir_Excel" runat="server" 
                             ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png" 
                             Width="24px" CssClass="Img_Button" 
                             AlternateText="Imprimir Excel"
                             ToolTip="Exportar Excel" 
                              OnClick="Btn_Imprimir_Excel_Click" />
                              <asp:ImageButton ID="Btn_Imprimir" runat="server" 
                             ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png" 
                             Width="24px" CssClass="Img_Button" 
                             AlternateText="Imprimir PDF" 
                             ToolTip="Exportar PDF" onclick="Btn_Imprimir_Click" />  
                            <asp:ImageButton ID="Btn_Salir" runat="server" 
                                CssClass="Img_Button"
                                ToolTip="Salir" 
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                onclick="Btn_Salir_Click"/>
                         </td>                            
                     </tr>
                     <tr>
                        <td>&nbsp;
                        </td>
                     </tr>
                     <tr>
                        <td>
                             <table border="0" cellspacing="0" class="estilo_fuente" frame="border" width="99%">
                                <tr>
                                    <td colspan="4">
                                         <div>
                                             <br />
                                        </div>
                                    </td>
                                </tr>
                                <tr >
                                    <td>
                                        Partida STOCK
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="Cmb_Partidas_STOCK" runat="server" Width="80%">
                                        </asp:DropDownList>
                                    </td>
                                    
                                 </tr>
                                <tr>
                                   
                                    <td style="width:18%; text-align:left;">
                                    <asp:Label ID="Lbl_Fecha_Inicial" runat="server" Text="Fecha Inicial" CssClass="estilo_fuente"></asp:Label>
                                </td>
                                <td style="width:32%">
                                    <asp:TextBox ID="Txt_Fecha_Inicial" runat="server" Width="85%" MaxLength="20" Enabled="true"></asp:TextBox>
                                    <asp:ImageButton ID="Btn_Fecha_Inicial" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                                    <cc1:CalendarExtender ID="CE_Txt_Fecha_Inicial" runat="server" TargetControlID="Txt_Fecha_Inicial" PopupButtonID="Btn_Fecha_Inicial" Format="dd/MMM/yyyy">
                                    </cc1:CalendarExtender>
                                    <cc1:MaskedEditExtender ID="MEE_Txt_Fecha_Inicio" Mask="99/LLL/9999" runat="server" MaskType="None" 
                                            UserDateFormat="DayMonthYear" UserTimeFormat="None" Filtered="/" TargetControlID="Txt_Fecha_Inicial" 
                                            Enabled="True" ClearMaskOnLostFocus="false"/>  
                                    <cc1:MaskedEditValidator ID="MEV_MEE_Txt_Fecha_Inicio" runat="server" ControlToValidate="Txt_Fecha_Inicial" ControlExtender="MEE_Txt_Fecha_Inicio" 
                                            EmptyValueMessage="Fecha Requerida" InvalidValueMessage="Fecha no valida" IsValidEmpty="false" 
                                            TooltipMessage="Ingresar Fecha" Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>
                                </td>
                                <td style="width:18%; text-align:left;">
                                    <asp:Label ID="Lbl_Fecha_Final" runat="server" Text="Fecha Final" CssClass="estilo_fuente"></asp:Label>
                                </td>
                                <td style="width:32%" style="text-align:left;">
                                    <asp:TextBox ID="Txt_Fecha_Final" runat="server" Width="85%" MaxLength="20" Enabled="true"></asp:TextBox>
                                    <asp:ImageButton ID="Btn_Fecha_Final" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                                    <cc1:CalendarExtender ID="CE_Txt_Fecha_Final" runat="server" TargetControlID="Txt_Fecha_Final" PopupButtonID="Btn_Fecha_Final" Format="dd/MMM/yyyy">
                                    </cc1:CalendarExtender>
                                    <cc1:MaskedEditExtender ID="MaskedEditExtender1" Mask="99/LLL/9999" runat="server" MaskType="None" 
                                            UserDateFormat="DayMonthYear" UserTimeFormat="None" Filtered="/" TargetControlID="Txt_Fecha_Final" 
                                            Enabled="True" ClearMaskOnLostFocus="false"/>  
                                    <cc1:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlToValidate="Txt_Fecha_Final" ControlExtender="MEE_Txt_Fecha_Inicio" 
                                            EmptyValueMessage="Fecha Requerida" InvalidValueMessage="Fecha no valida" IsValidEmpty="false" 
                                            TooltipMessage="Ingresar Fecha" Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>
                                </td>
                                 </tr>
                               
                                
                                <tr>
                                    <td>
                                        <br />
                                        <br />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
              </div>
          </ContentTemplate>
          <Triggers>
            <asp:PostBackTrigger ControlID="Btn_Imprimir_Excel" />
          </Triggers>
       </asp:UpdatePanel>
</asp:Content>


<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Rpt_Alm_Requisiciones_Canceladas.aspx.cs" Inherits="paginas_Almacen_Frm_Rpt_Alm_Requisiciones_Canceladas" Title="Reporte de Requisiciones Canceladas" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
  <asp:ScriptManager ID="ScriptManager_Reportes" runat="server" EnableScriptGlobalization = "true" EnableScriptLocalization = "True" />
       <asp:UpdatePanel ID="Upd_Panel" runat="server" UpdateMode="Always">
             <ContentTemplate>
                <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                        <div  class="processMessage" id="div_progress">
                            <img alt="" src="../Imagenes/paginas/Updating.gif" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
             
               
                <div id="Div_Contenido" style="background-color:#ffffff; width:98%; height:100%;"> <%--Fin del div General--%>
                    <table  border="0" cellspacing="0" class="estilo_fuente" frame="border" >
                        <tr>
                            <td class="label_titulo">Reporte de Requisiciones De Stock Canceladas
                            </td>
                       </tr>
                        <tr> <!--Bloque del mensaje de error-->
                            <td>
                               <div id="Div_Contenedor_Msj_Error" style="width:95%;font-size:9px;" runat="server" visible="false">
                                  <table style="width:100%;">
                                    <tr>
                                        <td colspan="4">&nbsp;
                                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                                        </td>
                                    </tr> 
                                </table>                   
                              </div>
                            </td>
                        </tr>
                        <tr class="barra_busqueda">
                          <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>  
                                <td style="width:20%;">
                                    <asp:ImageButton ID="Btn_Imprimir_Excel" runat="server" 
                                         ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png" Width="24px" CssClass="Img_Button"  
                                         ToolTip="Exportar Excel"
                                         OnClick="Btn_Imprimir_Excel_Click" Visible="true"/> 
                                    <asp:ImageButton ID="Btn_Salir" runat="server" 
                                        CssClass="Img_Button" 
                                        ToolTip="Salir"
                                        ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                        onclick="Btn_Salir_Click"/>
                                 </td> 
                               </ContentTemplate>                           
                            </asp:UpdatePanel>   
                        </tr>                        
                         <tr>
                            <td>   
                                <table border="0" cellspacing="0" class="estilo_fuente" frame="border" width="99%">
                                    <tr >
                                        <td width="10%"></td>
                                        <td width="80%"></td>
                                        <td width="10%"></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Lbl_Fecha_Inicial" runat="server" Text="Fecha Inicial"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Txt_Fecha_Inicial" runat="server" Width="15%" MaxLength="20" Enabled="true"></asp:TextBox>
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
                                    </tr>
                                    
                                    <tr>
                                        <td>
                                            <asp:Label ID="Lbl_Fecha_Final" runat="server" Text="Fecha Final"></asp:Label>
                                        </td>
                                         <td>
                                            <asp:TextBox ID="Txt_Fecha_Final" runat="server" Width="15%" MaxLength="20" Enabled="true"></asp:TextBox>
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
                                        <td></td>
                                    </tr>
                                        <td></td>
                                    <tr>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                </table>
                </div>
             </ContentTemplate>
          <Triggers>
              <asp:PostBackTrigger  ControlID="Btn_Imprimir_Excel"/>
          </Triggers>              
      </asp:UpdatePanel>
</asp:Content>
<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Alm_Reportes_Contrarecibos.aspx.cs" Inherits="paginas_Almacen_Frm_Ope_Alm_Reportes_Contrarecibos" Title="Reporte Contra Recibos" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript" language="javascript">
                function calendarShown(sender, args){
                    sender._popupBehavior._element.style.zIndex = 10000005;
                }
    </script> 
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
                          <td class="label_titulo">Reporte de Contra Recibos
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
                            <asp:ImageButton ID="Btn_Imprimir" runat="server" 
                             ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png" Width="24px" CssClass="Img_Button" 
                             AlternateText="Imprimir PDF" 
                             ToolTip="Exportar PDF"
                             OnClick="Btn_Imprimir_Click"/>  
                             <asp:ImageButton ID="Btn_Imprimir_Excel" runat="server" 
                                 ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png" Width="24px" CssClass="Img_Button" 
                                 AlternateText="Imprimir Excel" 
                                 ToolTip="Exportar Excel"
                                 OnClick="Btn_Imprimir_Excel_Click" Visible="true"/> 
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
                                <tr >
                                    <td width="170px">
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td width="170px">
                                       &nbsp;&nbsp;&nbsp; <asp:CheckBox ID="Ckb_No_ContraRecibo" runat="server" 
                                            OnCheckedChanged="Ckb_No_ContraRecibo_CheckedChanged" Text="No. Contra Recibo" 
                                            Checked="false" 
                                            AutoPostBack="True"  />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="Cmb_ContraRecibos" runat="server" Width="400px" Enabled="False"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                 </tr>
                                <tr>
                                    <td width="170px">
                                       &nbsp;&nbsp;&nbsp; <asp:CheckBox ID="Ckb_Proveedor" runat="server" 
                                            OnCheckedChanged="Ckb_Proveedor_CheckedChanged" Text="Proveedor" 
                                            Checked="false" 
                                            AutoPostBack="True"  />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="Cmb_Proveedores" runat="server" Width="400px" Enabled="False"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                 </tr>
                                <tr>
                                    <td width="170px">
                                       &nbsp;&nbsp;&nbsp; <asp:CheckBox ID="Ckb_Usuario_Genero" runat="server" 
                                            OnCheckedChanged="Ckb_Usuario_Genero_CheckedChanged" Text="Usuario Generó" 
                                            Checked="false" 
                                            AutoPostBack="True"  />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="Cmb_Usuarios" runat="server" Width="400px" Enabled="False"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                 </tr>
                                <tr>
                                    <td width="170px" >
                                        &nbsp;&nbsp;&nbsp; 
                                        <asp:CheckBox ID="Ckb_Fecha_Recepcion"
                                        runat="server" OnCheckedChanged="Ckb_Fecha_Recepcion_CheckedChanged" 
                                        Text="Fecha Recepción" Checked="false" 
                                        AutoPostBack="True"  />
                                    </td>
                                    <td >
                                        <asp:TextBox ID="Txt_Fecha_Inicial_B" runat="server" 
                                        Enabled="False" Width="160px"></asp:TextBox>
                                        <asp:ImageButton ID="Btn_Calendar_Fecha_Inicial" runat="server" 
                                        ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" Enabled="False" 
                                            ToolTip="Seleccione la Fecha Inicial" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;  
                                            
                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="Txt_Fecha_Inicial_B" 
                                        PopupButtonID="Btn_Calendar_Fecha_Inicial" OnClientShown="calendarShown" Format="dd/MMM/yyyy">
                                        </cc1:CalendarExtender>  
                                            
                                        <asp:TextBox ID="Txt_Fecha_Final_B" runat="server"  
                                        Enabled="False" Width="160px"></asp:TextBox>
                                        <asp:ImageButton ID="Btn_Calendar_Fecha_Final" runat="server" 
                                        ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" Enabled="False" 
                                            ToolTip="Seleccione la Fecha Final" />
                                            
                                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="Txt_Fecha_Final_B" 
                                        PopupButtonID="Btn_Calendar_Fecha_Final" OnClientShown="calendarShown" Format="dd/MMM/yyyy">
                                        </cc1:CalendarExtender>  
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
      </asp:UpdatePanel>
</asp:Content>


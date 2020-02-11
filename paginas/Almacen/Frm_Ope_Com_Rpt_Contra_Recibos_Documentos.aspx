<%@ Page Language="C#"  MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Com_Rpt_Contra_Recibos_Documentos.aspx.cs" Inherits="paginas_Almacen_Frm_Ope_Com_Rpt_Contra_Recibos_Documentos" Title="Reporte de Contra Recibos Documentos "%>
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
               <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                     <ContentTemplate>                
                  
                    <table  border="0" cellspacing="0" class="estilo_fuente" frame="border" style="width:100%;">
                        <tr>
                            <td class="label_titulo">Reporte de Relaci&oacute;n de Documentos Enviados
                            </td>
                       </tr>
                        <tr> <!--Bloque del mensaje de error-->
                            <td>
                                <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                                <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                            </td>      
                        </tr>
                        
                        <tr class="barra_busqueda">
                            <td style="width:20%;">
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
                     </table>
                    </ContentTemplate>
              </asp:UpdatePanel>                     
                     <table border="0" cellspacing="0" class="estilo_fuente" frame="border" width="99%">   
                        <tr>
                           
                            <td>   
                                <table border="0" cellspacing="0" class="estilo_fuente" frame="border" width="99%">
                                    <tr >
                                    
                                        <td width="25%" ></td>
                                        <td width="40%" ></td>
                                        <td width="15%" ></td>
                                        <td width="15%" ></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="button_agregar" >
                                        Generar reporte de contra recibo por
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="4"></td>
                                    </tr>
                                   <tr>
                                        <td colspan="2" class="button_agregar">
                                            <asp:CheckBox ID="Chk_Fechas" runat="server" Text="Fecha"  Width="60%" 
                                                AutoPostBack="true" OnCheckedChanged="Chk_Fechas_OnCheckedChanged"  />
                                        </td>
                                        
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Lbl_Fecha_Inicial" runat="server" Text="Fecha Inicial"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Txt_Fecha_Inicial" runat="server" Width="60%" Enabled="true" ></asp:TextBox>
                                            <cc1:CalendarExtender ID="Txt_Fecha_Inicial_CalendarExtender" runat="server" 
                                                TargetControlID="Txt_Fecha_Inicial" PopupButtonID="Btn_Fecha_Inicial" Format="dd/MMM/yyyy" />
                                            <asp:ImageButton ID="Btn_Fecha_Inicial" runat="server" 
                                                ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" ToolTip="Seleccione la Fecha Inicial" />
                                            <cc1:MaskedEditExtender ID="MEE_Txt_Fecha_Inicio" Mask="99/LLL/9999" runat="server" MaskType="None" 
                                                    UserDateFormat="DayMonthYear" UserTimeFormat="None" Filtered="/" TargetControlID="Txt_Fecha_Inicial" 
                                                    Enabled="True" ClearMaskOnLostFocus="false"/>  
                                            <cc1:MaskedEditValidator ID="MEV_MEE_Txt_Fecha_Inicio" runat="server" ControlToValidate="Txt_Fecha_Inicial" ControlExtender="MEE_Txt_Fecha_Inicio" 
                                                    EmptyValueMessage="Fecha Requerida" InvalidValueMessage="Fecha no valida" IsValidEmpty="false" 
                                                    TooltipMessage="Ingresar Fecha" Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>  
                                                                                                 
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td >
                                            <asp:Label ID="Lbl_Fecha_Final" runat="server" Text="Fecha Final" ></asp:Label>
                                        </td>
                                         <td>
                                            <asp:TextBox ID="Txt_Fecha_Final" runat="server" Width="60%" Enabled="true"></asp:TextBox>
                                            <cc1:CalendarExtender ID="CalendarExtender3" runat="server" 
                                                TargetControlID="Txt_Fecha_Final" PopupButtonID="Btn_Fecha_Final" Format="dd/MMM/yyyy" />
                                            <asp:ImageButton ID="Btn_Fecha_Final" runat="server" 
                                                ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" ToolTip="Seleccione la Fecha Final" />
                                            <cc1:MaskedEditExtender ID="MaskedEditExtender1" Mask="99/LLL/9999" runat="server" MaskType="None" 
                                                    UserDateFormat="DayMonthYear" UserTimeFormat="None" Filtered="/" TargetControlID="Txt_Fecha_Final" 
                                                    Enabled="True" ClearMaskOnLostFocus="false"/>  
                                            <cc1:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlToValidate="Txt_Fecha_Final" ControlExtender="MEE_Txt_Fecha_Inicio" 
                                                    EmptyValueMessage="Fecha Requerida" InvalidValueMessage="Fecha no valida" IsValidEmpty="false" 
                                                    TooltipMessage="Ingresar Fecha" Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>                             
                                         </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="button_agregar">
                                            <asp:CheckBox ID="Chk_Numero" runat="server" Text="Número contra recibo" 
                                                AutoPostBack="true" OnCheckedChanged="Chk_Numero_OnCheckedChanged" Width="100%" />
                                        </td>
                                        
                                    </tr>
                                    
                                    <tr>
                                        <td>
                                            <asp:Label ID="Lbl_Numero_Contrarecibo" runat="server" Text="Número Contra Recibo" ></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Txt_Numero_Contra_Recibo" runat="server" Width="60%"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Numero_Contra_Recibo" runat="server" 
                                                TargetControlID="Txt_Numero_Contra_Recibo" FilterType="Custom, Numbers" 
                                                ValidChars="0123456789" Enabled="True"> 
                                            </cc1:FilteredTextBoxExtender> 
                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="button_agregar">
                                            <asp:CheckBox ID="Chk_Rango_Numeros" runat="server" Text="Rango de numeros de contra recibo" 
                                                AutoPostBack="true" OnCheckedChanged="Chk_Rango_Numeros_OnCheckedChanged" Width="100%" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Lbl_Numero_Inicial" runat="server" Text="Número Contra Recibo Inicial" ></asp:Label>
                                        </td>
                                        
                                        <td>
                                            <asp:TextBox ID="Txt_Numero_Inicial" runat="server" Width="60%"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Numero_Inicial" runat="server" 
                                                TargetControlID="Txt_Numero_Inicial" FilterType="Custom, Numbers" 
                                                ValidChars="0123456789" Enabled="True"> 
                                            </cc1:FilteredTextBoxExtender> 
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td>
                                            <asp:Label ID="Lbl_Numero_Final" runat="server" Text="Número Contra Recibo Final" ></asp:Label>
                                        </td>
                                        <td>
                                        
                                            <asp:TextBox ID="Txt_Numero_Final" runat="server" Width="60%"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Numero_Final" runat="server" 
                                                TargetControlID="Txt_Numero_Final" FilterType="Custom, Numbers" 
                                                ValidChars="0123456789" Enabled="True"> 
                                            </cc1:FilteredTextBoxExtender> 
                                        </td>
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
    


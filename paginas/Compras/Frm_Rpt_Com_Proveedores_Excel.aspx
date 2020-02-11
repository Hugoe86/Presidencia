<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Rpt_Com_Proveedores_Excel.aspx.cs" Inherits="paginas_Compras_Frm_Rpt_Com_Proveedores_Excel" Title="Reporte de Proveedores"%>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server" >
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
           
                <div id="Div_Contenido" style="background-color:#ffffff; width:100%; height:100%;"> <%--Fin del div General--%>
                    <table  border="0" cellspacing="0" class="estilo_fuente" frame="border" style="width:100%;">
                        <tr>
                            <td class="label_titulo">Reporte de Proveedores
                            </td>
                       </tr>
                        <tr> <!--Bloque del mensaje de error-->
                            <td>
                                <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                                <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                            </td>      
                        </tr>
                    </table>       
                 </div>
                 
                 <div id="Div1" style="background-color:#ffffff; width:98%; height:100%;"> <%--Fin del div General--%>
                   <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
                      <ContentTemplate>                 

                        <table  border="0" cellspacing="0" class="estilo_fuente" frame="border" style="width:100%;" >
                            <tr class="barra_busqueda" style="width:100%;">
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
                 </div>
            
                 <div id="Div2" style="background-color:#ffffff; width:98%; height:100%;"> <%--Fin del div General--%>       
<%--                     <table  border="0" cellspacing="0" class="estilo_fuente" frame="border" style="width:100%;">
                        <tr style="width:100%;">                           
                            <td >   --%>
                                <table border="0" cellspacing="0" class="estilo_fuente" frame="border" width="100%">
                                    <tr >                                    
                                        <td width="15%" ></td>
                                        <td width="40%" ></td>
                                        <td width="15%" ></td>
                                        <td width="30%" ></td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" class="button_agregar" >
                                        Generar reporte de proveedor
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="4"></td>
                                    </tr>
                                   <tr>
                                        <td colspan="4" class="button_agregar">
                                            <asp:CheckBox ID="Chk_Partida" runat="server" Text="Partida"  Width="99%"
                                               AutoPostBack="true" OnCheckedChanged="Chk_Partida_OnCheckedChanged"  />
                                        </td>
                                    </tr>
                                    <tr style="width:100%;">
                                        <td>
                                            <asp:Label ID="Lbl_Partida" runat="server" Text="Partida"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="Cmb_Partida" runat="server" Width="100%"></asp:DropDownList>
                                        </td>
                                  
                                    </tr>
                                    <tr>
                                        <td colspan="4"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" class="button_agregar">
                                            <asp:CheckBox ID="Chk_Compras_Otorgadas" runat="server" Text="Por más compras otorgadas"  Width="99%" 
                                               AutoPostBack="true" OnCheckedChanged="Chk_Compras_Otorgadas_OnCheckedChanged"  />
                                        </td>
                                    </tr>
                                     <tr>
                                        <td>
                                            <asp:Label ID="Lbl_Fecha_Inicial" runat="server" Text="Fecha Inicial"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Txt_Fecha_Inicial" runat="server" Width="120px" Enabled="true" ></asp:TextBox>
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
                                            <asp:TextBox ID="Txt_Fecha_Final" runat="server" Width="120px" Enabled="true"></asp:TextBox>
                                            <cc1:CalendarExtender ID="CalendarExtender3" runat="server" 
                                                TargetControlID="Txt_Fecha_Final" PopupButtonID="Btn_Fecha_Final" Format="dd/MMM/yyyy" />
                                            <asp:ImageButton ID="Btn_Fecha_Final" runat="server" 
                                                ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" ToolTip="Seleccione la Fecha Final" />
                                            <cc1:MaskedEditExtender ID="MaskedEditExtender1" Mask="99/LLL/9999" runat="server" MaskType="None" 
                                                    UserDateFormat="DayMonthYear" UserTimeFormat="None" Filtered="/" TargetControlID="Txt_Fecha_Final" 
                                                    Enabled="True" ClearMaskOnLostFocus="false"/>  
                                            <cc1:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlToValidate="Txt_Fecha_Final" ControlExtender="MEE_Txt_Fecha_Inicio" 
                                                    EmptyValueMessage="Fecha Requerida" InvalidValueMessage="Fecha no valida" IsValidEmpty="false" 
                                                    TooltipMessage="Ingresar Fecha" Enabled="true" style="font-size:10px;background-color:#F0F8FF;
                                                    color:Black;font-weight:bold;"/>
                                         </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" class="button_agregar">
                                            <asp:CheckBox ID="Chk_Fecha_Registro" runat="server" Text="Por fecha de Registro"  Width="99%" 
                                               AutoPostBack="true" OnCheckedChanged="Chk_Fecha_Registro_OnCheckedChanged" />
                                        </td>
                                    </tr>
                                     <tr>
                                        <td>
                                            <asp:Label ID="Lbl_Fecha_Ini" runat="server" Text="Fecha Inicial"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Txt_Fecha_Ini" runat="server" Width="120px" Enabled="true" ></asp:TextBox>
                                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" 
                                                TargetControlID="Txt_Fecha_Ini" PopupButtonID="Btn_Fecha_Ini" Format="dd/MMM/yyyy" />
                                            <asp:ImageButton ID="Btn_Fecha_Ini" runat="server" 
                                                ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" ToolTip="Seleccione la Fecha Inicial" />
                                            <cc1:MaskedEditExtender ID="MaskedEditExtender2" Mask="99/LLL/9999" runat="server" MaskType="None" 
                                                    UserDateFormat="DayMonthYear" UserTimeFormat="None" Filtered="/" TargetControlID="Txt_Fecha_Inicial" 
                                                    Enabled="True" ClearMaskOnLostFocus="false"/>  
                                            <cc1:MaskedEditValidator ID="MaskedEditValidator2" runat="server" ControlToValidate="Txt_Fecha_Ini" ControlExtender="MaskedEditExtender2" 
                                                    EmptyValueMessage="Fecha Requerida" InvalidValueMessage="Fecha no valida" IsValidEmpty="false" 
                                                    TooltipMessage="Ingresar Fecha" Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td >
                                            <asp:Label ID="Lbl_Fecha_Fin" runat="server" Text="Fecha Final" ></asp:Label>
                                        </td>
                                         <td>
                                            <asp:TextBox ID="Txt_Fecha_Fin" runat="server" Width="120px" Enabled="true"></asp:TextBox>
                                            <cc1:CalendarExtender ID="CalendarExtender2" runat="server" 
                                                TargetControlID="Txt_Fecha_Fin" PopupButtonID="Btn_Fecha_Fin" Format="dd/MMM/yyyy" />
                                            <asp:ImageButton ID="Btn_Fecha_Fin" runat="server" 
                                                ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" ToolTip="Seleccione la Fecha Final" />
                                            <cc1:MaskedEditExtender ID="MaskedEditExtender3" Mask="99/LLL/9999" runat="server" MaskType="None" 
                                                    UserDateFormat="DayMonthYear" UserTimeFormat="None" Filtered="/" TargetControlID="Txt_Fecha_Fin" 
                                                    Enabled="True" ClearMaskOnLostFocus="false"/>  
                                            <cc1:MaskedEditValidator ID="MaskedEditValidator3" runat="server" ControlToValidate="Txt_Fecha_Fin" ControlExtender="MaskedEditExtender3" 
                                                    EmptyValueMessage="Fecha Requerida" InvalidValueMessage="Fecha no valida" IsValidEmpty="false" 
                                                    TooltipMessage="Ingresar Fecha" Enabled="true" style="font-size:10px;background-color:#F0F8FF;
                                                    color:Black;font-weight:bold;"/>
                                         </td>
                                    </tr>
                                      <tr>
                                        <td colspan="4" class="button_agregar">
                                            <asp:CheckBox ID="Chk_Fecha_Actualizacion" runat="server" 
                                                Text="Por fecha de Actualizacion"  Width="99%" 
                                               AutoPostBack="true" 
                                                oncheckedchanged="Chk_Fecha_Actualizacion_CheckedChanged" />
                                        </td>
                                    </tr>
                                     <tr>
                                        <td>
                                            <asp:Label ID="Lbl_Fecha_Ini_Act" runat="server" Text="Fecha Inicial"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Txt_Fecha_Ini_Act" runat="server" Width="120px" Enabled="true" ></asp:TextBox>
                                            <cc1:CalendarExtender ID="CalendarExtender4" runat="server" 
                                                TargetControlID="Txt_Fecha_Ini_Act" PopupButtonID="Btn_Fecha_Ini_Act" Format="dd/MMM/yyyy" />
                                            <asp:ImageButton ID="Btn_Fecha_Ini_Act" runat="server" 
                                                ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" ToolTip="Seleccione la Fecha Inicial" />
                                            <cc1:MaskedEditExtender ID="MaskedEditExtender4" Mask="99/LLL/9999" runat="server" MaskType="None" 
                                                    UserDateFormat="DayMonthYear" UserTimeFormat="None" Filtered="/" TargetControlID="Txt_Fecha_Ini_Act" 
                                                    Enabled="True" ClearMaskOnLostFocus="false"/>  
                                            <cc1:MaskedEditValidator ID="MaskedEditValidator4" runat="server" ControlToValidate="Txt_Fecha_Ini_Act" ControlExtender="MaskedEditExtender4" 
                                                    EmptyValueMessage="Fecha Requerida" InvalidValueMessage="Fecha no valida" IsValidEmpty="false" 
                                                    TooltipMessage="Ingresar Fecha" Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td >
                                            <asp:Label ID="Lbl_Fecha_Fin_Act" runat="server" Text="Fecha Final" ></asp:Label>
                                        </td>
                                         <td>
                                            <asp:TextBox ID="Txt_Fecha_Fin_Act" runat="server" Width="120px" Enabled="true"></asp:TextBox>
                                            <cc1:CalendarExtender ID="CalendarExtender5" runat="server" 
                                                TargetControlID="Txt_Fecha_Fin_Act" PopupButtonID="Btn_Fecha_Fin_Act" Format="dd/MMM/yyyy" />
                                            <asp:ImageButton ID="Btn_Fecha_Fin_Act" runat="server" 
                                                ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" ToolTip="Seleccione la Fecha Final" />
                                            <cc1:MaskedEditExtender ID="MaskedEditExtender5" Mask="99/LLL/9999" runat="server" MaskType="None" 
                                                    UserDateFormat="DayMonthYear" UserTimeFormat="None" Filtered="/" TargetControlID="Txt_Fecha_Fin_Act" 
                                                    Enabled="True" ClearMaskOnLostFocus="false"/>  
                                            <cc1:MaskedEditValidator ID="MaskedEditValidator5" runat="server" ControlToValidate="Txt_Fecha_Fin_Act" ControlExtender="MaskedEditExtender5" 
                                                    EmptyValueMessage="Fecha Requerida" InvalidValueMessage="Fecha no valida" IsValidEmpty="false" 
                                                    TooltipMessage="Ingresar Fecha" Enabled="true" style="font-size:10px;background-color:#F0F8FF;
                                                    color:Black;font-weight:bold;"/>
                                         </td>
                                    </tr>
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
 <%--                           </td>
                        </tr>
                    </table>--%>
                </div>
            </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger  ControlID="Btn_Imprimir_Excel"/>
        </Triggers>            
      </asp:UpdatePanel>
</asp:Content>
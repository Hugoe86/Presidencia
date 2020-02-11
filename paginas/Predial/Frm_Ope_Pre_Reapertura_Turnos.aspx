<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Pre_Reapertura_Turnos.aspx.cs" Inherits="paginas_Predial_Frm_Ope_Pre_Reapertura_Turnos" Title="Untitled Page" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">

<asp:ScriptManager ID="Sm_Reapertura_Dia" runat="server" EnableScriptGlobalization = "true" EnableScriptLocalization = "True"/>
    <asp:UpdatePanel ID="Upnl_Reapertura_Dia" runat="server">
        <ContentTemplate>
        
            <asp:UpdateProgress ID="UPgrs_Reaperrura_Dia" runat="server" AssociatedUpdatePanelID="Upnl_Reapertura_Dia" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        
            <div id="Div_Contenedor_Principal" style ="width:98%; background-color:White;">
            
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo">Reapertura de Turno</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false"  />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"/>
                        </td>
                    </tr>
                </table> 
                
                <table width="100%"  border="0" cellspacing="0">
                     <tr align="center">
                         <td colspan="2">                
                             <div align="right" class="barra_busqueda">                        
                                  <table style="width:100%;height:28px;">
                                    <tr>
                                      <td align="left" style="width:59%;">                                                  
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" TabIndex="6"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" />
                                      </td>
                                      <td align="right" style="width:41%;">
                                        <table style="width:100%;height:28px;">
                                            <tr>
                                                <td style="width:60%;vertical-align:top;">                                
                                                </td>
                                            </tr>                                                                          
                                        </table>                                    
                                       </td>       
                                     </tr>         
                                  </table>                      
                                </div>
                         </td>
                     </tr>
                </table>    
                
                <asp:Panel ID="Pnl_Filtros_Busqueda" runat="server" GroupingText="Opciones de Búsqueda">
                    <table width="100%">
                        <tr>
                            <td colspan="4">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td style="width:20%; text-align:left; cursor:default;">
                                Fecha Inicio
                            </td>
                            <td style="width:30%; text-align:left; cursor:default;">
                                <asp:TextBox ID="Txt_Fecha_Inicio" runat="server"  Width="88%"/>
                                <cc1:FilteredTextBoxExtender ID="Txt_Fecha_Inicio_FilteredTextBoxExtender" 
                                    runat="server" TargetControlID="Txt_Fecha_Inicio" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" ValidChars="/_"/>
                                <cc1:CalendarExtender ID="Txt_Fecha_Inicio_CalendarExtender" runat="server" 
                                    TargetControlID="Txt_Fecha_Inicio" PopupButtonID="Btn_Fecha_Inicio" Format="dd/MMM/yyyy"/>
                                <asp:ImageButton ID="Btn_Fecha_Inicio" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                    ToolTip="Seleccione la Fecha Final" style="cursor:hand;"/>
                                <cc1:MaskedEditExtender 
                                    ID="Mee_Txt_Fecha_Inicio" 
                                    Mask="99/LLL/9999" 
                                    runat="server"
                                    MaskType="None" 
                                    UserDateFormat="DayMonthYear" 
                                    UserTimeFormat="None" Filtered="/"
                                    TargetControlID="Txt_Fecha_Inicio" 
                                    Enabled="True" 
                                    ClearMaskOnLostFocus="false"/>  
                                <cc1:MaskedEditValidator 
                                    ID="Mev_Mee_Txt_Fecha_Inicio" 
                                    runat="server" 
                                    ControlToValidate="Txt_Fecha_Inicio"
                                    ControlExtender="Mee_Txt_Fecha_Inicio" 
                                    EmptyValueMessage="La Fecha Inicial es obligatoria"
                                    InvalidValueMessage="Fecha Inicial Invalida" 
                                    IsValidEmpty="true" 
                                    TooltipMessage="Ingrese o Seleccione la Fecha Inicial"
                                    Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>  
                            </td>
                            <td style="width:20%; text-align:left; cursor:default;">
                                &nbsp;&nbsp;Fecha Final
                            </td>
                            <td style="width:30%; text-align:left; cursor:default;">
                                <asp:TextBox ID="Txt_Fecha_Fin" runat="server" Width="88%" />
                                <cc1:FilteredTextBoxExtender ID="Txt_Fecha_Fin_FilteredTextBoxExtender" 
                                    runat="server" TargetControlID="Txt_Fecha_Fin" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" ValidChars="/_"/>
                                <cc1:CalendarExtender ID="Txt_Fecha_Fin_CalendarExtender" runat="server" 
                                    TargetControlID="Txt_Fecha_Fin" PopupButtonID="Btn_Fecha_Fin" Format="dd/MMM/yyyy" />
                                <asp:ImageButton ID="Btn_Fecha_Fin" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                    ToolTip="Seleccione la Fecha Inicial" style="cursor:hand;"/>   
                                <cc1:MaskedEditExtender 
                                    ID="Mee_Txt_Fecha_Fin" 
                                    Mask="99/LLL/9999" 
                                    runat="server"
                                    MaskType="None" 
                                    UserDateFormat="DayMonthYear" 
                                    UserTimeFormat="None" Filtered="/"
                                    TargetControlID="Txt_Fecha_Fin" 
                                    Enabled="True" 
                                    ClearMaskOnLostFocus="false"/>  
                                <cc1:MaskedEditValidator 
                                    ID="Mev_Mee_Txt_Fecha_Fin" 
                                    runat="server" 
                                    ControlToValidate="Txt_Fecha_Fin"
                                    ControlExtender="Mee_Txt_Fecha_Fin" 
                                    EmptyValueMessage="La Fecha Final es obligatoria"
                                    InvalidValueMessage="Fecha Final Invalida" 
                                    IsValidEmpty="true" 
                                    TooltipMessage="Ingrese o Seleccione la Fecha Final"
                                    Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>  
                            </td>                        
                        </tr>
                        <tr>
                            <td colspan="4">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align:left; width:50%;">
                                <asp:Button ID="Btn_Abrir_Cierre_Dia" runat="server" Text="Reapertura Turno" Enabled="false"
                                    OnClick="Btn_Abrir_Cierre_Dia_Click" style="width:150px; height:32px;"/>
                            </td>
                            <td colspan="2" style="text-align:right; width:50%;">
                                <asp:ImageButton ID="Btn_Busqueda_Cierre_Dias" runat="server" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png"
                                    OnClick="Btn_Busqueda_Cierre_Dias_Click" style="width:32px; height:32px;"/>
                            </td>
                        </tr>                    
                    </table>
                </asp:Panel>
                
                <table width="100%">
                    <tr style="background-color: #36C;">
                        <td style="text-align:left; font-size:14px; color:#FFF; font-weight:bolder;" 
                                colspan="4" >
                            Datos de Reapertura
                        </td>
                    </tr>
                    <tr>
                        <td width="15%">*Autorizo Reapertura</td>
                        <td width="85%" colspan = "3">
                            <asp:TextBox ID="Txt_Autorizo_Reapertura" MaxLength="100" 
                                runat="server" Width="99%" Enabled="False"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td width="15%">*Observaciones</td>
                        <td width="85%" colspan = "3">
                            <asp:TextBox ID="Txt_Observaciones" MaxLength="255" runat="server" 
                                Height="62px" Width="99%" Enabled="False"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td style="width:100%;" colspan="4">
                            <asp:GridView ID="Grid_Cierres_Turno_Dia" runat="server" CssClass="GridView_1" 
                                Width="100%" AutoGenerateColumns="False"  
                                GridLines="None" HeaderStyle-CssClass="tblHead" 
                                OnRowDataBound="Grid_Cierres_Turno_Dia_RowDataBound" PageSize="5" OnPageIndexChanging="Grid_Cierres_Turno_Dia_PageIndexChanging"
                                OnSelectedIndexChanged="Grid_Cierres_Turno_Dia_SelectedIndexChanged" 
                                EnableModelValidation="True">
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select"  HeaderText=""
                                            ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                            <ItemStyle Width="5%"  HorizontalAlign="Left"/>
                                            <HeaderStyle HorizontalAlign="Left" Width="5%"/>
                                        </asp:ButtonField>                                                
                                        <asp:BoundField DataField="NO_TURNO_DIA" HeaderText="No Cierre">
                                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                            <ItemStyle HorizontalAlign="Left" Width="15%" />
                                        </asp:BoundField>                                                   
                                        <asp:BoundField DataField="FECHA_TURNO" HeaderText="Fecha" 
                                            DataFormatString="{0:dd/MMM/yyyy}">
                                            <ControlStyle Width="15%" />
                                            <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                            <ItemStyle HorizontalAlign="Center" Width="15%" />
                                        </asp:BoundField>  
                                        <asp:BoundField DataField="Hora_Apertura" DataFormatString="{0:HH:mm:ss}" 
                                            HeaderText="Hora Apertura">
                                        <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                        <ItemStyle HorizontalAlign="Center" Width="20%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Hora_Cierre" DataFormatString="{0:HH:mm:ss}" 
                                            HeaderText="Hora Cierre">
                                        <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                        <ItemStyle HorizontalAlign="Center" Width="20%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                            <HeaderStyle HorizontalAlign="Left" Width="65%" />
                                            <ItemStyle HorizontalAlign="Left" Width="65%" />
                                        </asp:BoundField>                                                                                                                                          
                                    </Columns>
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <HeaderStyle CssClass="tblHead" />
                                    <PagerStyle CssClass="GridHeader" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>  
                        </td>                      
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    
</asp:Content>


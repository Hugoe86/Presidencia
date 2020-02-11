<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Rpt_Caj_Pagos.aspx.cs" Inherits="paginas_Ingresos_Rpt_Caj_Pagos" Title="Untitled Page" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<asp:ScriptManager ID="SM_Rpt_Ingresos_Diarios" runat="server"  EnableScriptGlobalization = "true" EnableScriptLocalization = "True"/>
<asp:UpdatePanel ID="UPnl_Rpt_Ingresos_Diarios" runat="server" UpdateMode="Always">
    <ContentTemplate>
    
        <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="UPnl_Rpt_Ingresos_Diarios" DisplayAfter="0">
            <ProgressTemplate>
                <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
            </ProgressTemplate>
        </asp:UpdateProgress>  
        
        <div style="width:98%; background-color:White;">
            <table width="100%" title="Control_Errores"> 
                <tr>
                    <td class="button_autorizar" style="width:100%; text-align:center; cursor:default; font-size:14px;">
                        Reporte Ingresos Diarios de Presidencia
                    </td>
                </tr>
                <tr>
                    <td style="width:100%; text-align:left; cursor:default;">
                        <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />
                        <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"/>        
                    </td>
                </tr>
            </table>

            <asp:Panel ID="Pnl_Filtros" runat="server" Width="100%">        
                <table width="100%">                    
                    <tr>
                        <td class="button_autorizar" style="width:20%;text-align:left; cursor:default;">
                            Fecha Inicio
                        </td>
                        <td class="button_autorizar" style="width:30%;text-align:left; cursor:default;">
                            <asp:TextBox ID="Txt_Busqueda_Fecha_Inicio" runat="server" Width="85%" MaxLength="1" TabIndex="14"/>
                            <cc1:FilteredTextBoxExtender ID="FTxt_Busqueda_Fecha_Inicio" 
                                runat="server" TargetControlID="Txt_Busqueda_Fecha_Inicio" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                ValidChars="/_"/>
                            <cc1:CalendarExtender ID="CE_Txt_Busqueda_Fecha_Inicio" runat="server" 
                                TargetControlID="Txt_Busqueda_Fecha_Inicio" PopupButtonID="Btn_Busqueda_Fecha_Inicio" Format="dd/MMM/yyyy" 
                                />
                            <asp:ImageButton ID="Btn_Busqueda_Fecha_Inicio" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                ToolTip="Seleccione la Fecha" style="cursor:hand;"/> 
                            <cc1:MaskedEditExtender 
                                ID="Mee_Txt_Busqueda_Fecha_Inicio" 
                                Mask="99/LLL/9999" 
                                runat="server"
                                MaskType="None" 
                                UserDateFormat="DayMonthYear" 
                                UserTimeFormat="None" Filtered="/"
                                TargetControlID="Txt_Busqueda_Fecha_Inicio" 
                                Enabled="True" 
                                ClearMaskOnLostFocus="false"/>  
                            <cc1:MaskedEditValidator 
                                ID="Mev_Txt_Busqueda_Fecha_Inicio" 
                                runat="server" 
                                ControlToValidate="Txt_Busqueda_Fecha_Inicio"
                                ControlExtender="Mee_Txt_Busqueda_Fecha_Inicio" 
                                EmptyValueMessage="Es valido no ingresar fecha inicial"
                                InvalidValueMessage="Fecha Inicial Invalida" 
                                IsValidEmpty="true" 
                                TooltipMessage="Ingrese o Seleccione la Fecha Inicial"
                                Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>
                        </td>
                        <td class="button_autorizar" style="width:20%;text-align:left; cursor:default;">
                            &nbsp;&nbsp;Fecha Fin
                        </td>
                        <td class="button_autorizar" style="width:30%;text-align:left; cursor:default;">
                            <asp:TextBox ID="Txt_Busqueda_Fecha_Fin" runat="server" Width="85%" MaxLength="1" TabIndex="15"/>
                            <cc1:FilteredTextBoxExtender ID="FTxt_Busqueda_Fecha_Fin" 
                                runat="server" TargetControlID="Txt_Busqueda_Fecha_Fin" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                ValidChars="/_"/>
                            <cc1:CalendarExtender ID="CE_Txt_Busqueda_Fecha_Fin" runat="server" 
                                TargetControlID="Txt_Busqueda_Fecha_Fin" PopupButtonID="Btn_Busqueda_Fecha_Fin" Format="dd/MMM/yyyy" 
                                />
                            <asp:ImageButton ID="Btn_Busqueda_Fecha_Fin" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                ToolTip="Seleccione la Fecha" style="cursor:hand;"/> 
                            <cc1:MaskedEditExtender 
                                ID="Mee_Txt_Busqueda_Fecha_Fin" 
                                Mask="99/LLL/9999" 
                                runat="server"
                                MaskType="None" 
                                UserDateFormat="DayMonthYear" 
                                UserTimeFormat="None" Filtered="/"
                                TargetControlID="Txt_Busqueda_Fecha_Fin" 
                                Enabled="True" 
                                ClearMaskOnLostFocus="false"/>  
                            <cc1:MaskedEditValidator 
                                ID="Mev_Mee_Txt_Busqueda_Fecha_Fin" 
                                runat="server" 
                                ControlToValidate="Txt_Busqueda_Fecha_Fin"
                                ControlExtender="Mee_Txt_Busqueda_Fecha_Fin" 
                                EmptyValueMessage="Es valido no ingresar fecha final"
                                InvalidValueMessage="Fecha Final Invalida" 
                                IsValidEmpty="true" 
                                TooltipMessage="Ingrese o Seleccione la Fecha Final"
                                Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>
                        </td>
                    </tr>   
                </table>
            </asp:Panel>
                
            <table width="100%">
                <tr>
                    <td class="button_autorizar" style="width:100%; text-align:left; cursor:default;" colspan="4">
                       <hr />
                    </td>
                </tr>
                <tr>
                    <td style="width:100%; text-align:left; cursor:default;" colspan="4">
                        <asp:Button ID="Btn_Generar_Reporte" runat="server" Text="Consultar Ingresos Diarios" 
                            Width="100%" CssClass="button_autorizar" OnClick="Btn_Generar_Reporte_Click"/>
                    </td>
                </tr>   
                <tr>
                    <td style="width:100%; text-align:left; cursor:default;" colspan="4">
                        <hr />
                    </td>
                </tr>
            </table>
            
            <asp:GridView ID="Grid_Ingresos_Diarios" runat="server" CssClass="GridView_1" Width="99%"
                 AutoGenerateColumns="False"  GridLines="None"  HeaderStyle-CssClass="tblHead" AllowPaging ="true"
                 PageSize="20" OnPageIndexChanging="Grid_Ingresos_Diarios_PageIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="CLAVE" HeaderText="CLAVE">
                            <HeaderStyle HorizontalAlign="Left" Width="20%" Font-Names="Arial" Font-Size="11px" Font-Bold="true"/>
                            <ItemStyle HorizontalAlign="Left" Width="20%" Font-Names="Courier" Font-Size="8px" Font-Bold="true"/>
                        </asp:BoundField>
                        <asp:BoundField DataField="NOMBRE" HeaderText="NOMBRE">
                            <HeaderStyle HorizontalAlign="Left" Width="45%" Font-Names="Arial" Font-Size="11px" Font-Bold="true"/>
                            <ItemStyle HorizontalAlign="Left" Width="45%" Font-Names="Courier" Font-Size="9px" Font-Bold="false"/>
                        </asp:BoundField>
                        <asp:BoundField DataField="CANTIDAD" HeaderText="CANTIDAD">
                            <HeaderStyle HorizontalAlign="Center" Width="15%" Font-Names="Arial" Font-Size="11px" Font-Bold="true"/>
                            <ItemStyle HorizontalAlign="Center" Width="15%" Font-Names="Courier" Font-Size="8px" Font-Bold="true"/>
                        </asp:BoundField>  
                        <asp:BoundField DataField="MONTO" HeaderText="MONTO" DataFormatString="{0:c}">
                            <HeaderStyle HorizontalAlign="Left" Width="20%" Font-Names="Arial" Font-Size="11px" Font-Bold="true"/>
                            <ItemStyle HorizontalAlign="Left" Width="20%" Font-Names="Courier" Font-Size="8px" Font-Bold="true"/>
                        </asp:BoundField>
                    </Columns>
                    <SelectedRowStyle CssClass="GridSelected" />
                    <PagerStyle CssClass="GridHeader" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
            </asp:GridView>

            <table width="99.5%">
                <tr>
                    <td class="button_autorizar" style="width:100%; text-align:right; cursor:default;" colspan="4">
                        <asp:ImageButton ID="IBtn_PDF" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png"
                            OnClick="IBtn_PDF_Click" style="cursor:hand;" ToolTip="Mostrar Reporte de Ingresos Diarios"/>
                    </td>
                </tr>
            </table>
            <br /><br /><br />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>


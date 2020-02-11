<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Nom_Calendario_Reloj_Checador.aspx.cs" Inherits="paginas_Nomina_Frm_Cat_Nom_Calendario_Reloj_Checador" Title="Calendario Reloj Checador" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
    <script type="text/javascript" language="javascript">
        //Metodo para mantener los calendarios en una capa mas alat.
        function calendarShown(sender, args)
        {
            sender._popupBehavior._element.style.zIndex = 10000005;
        } 
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager_Areas" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>        
            <asp:UpdateProgress ID="Uprg_Reloj_Checador" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Calendario_Reloj_Checador" style="background-color:#ffffff; width:100%; height:100%;">    
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo">Calendario de Reloj Checador</td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table width="100%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr class="barra_busqueda" align="right">
                        <td align="left" width="50%">
                            <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" 
                                CssClass="Img_Button" TabIndex="1"
                                ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" 
                                onclick="Btn_Modificar_Click"/>
                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" 
                                CssClass="Img_Button" TabIndex="2"
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                onclick="Btn_Salir_Click"/>
                            <asp:TextBox ID="Txt_Nomina_ID" runat="server" Visible="false" ReadOnly="true"></asp:TextBox>    
                        </td>
                        <td width="50%">Busqueda
                            <asp:TextBox ID="Txt_Busqueda_No_Nomina" runat="server" MaxLength="100" TabIndex="3" 
                                ToolTip="Buscar por Año" Width="180px"/>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_No_Nomina" runat="server" WatermarkCssClass="watermarked"
                                WatermarkText="<Ingrese el Año>" TargetControlID="Txt_Busqueda_No_Nomina"/>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_No_Nomina" runat="server" 
                                TargetControlID="Txt_Busqueda_No_Nomina" FilterType="Custom, Numbers">
                            </cc1:FilteredTextBoxExtender>
                            <asp:ImageButton ID="Btn_Buscar_Periodo_Nominal" runat="server" 
                                onclick="Btn_Buscar_Periodo_Nominal_Click" TabIndex="4" ToolTip="Consultar" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"/>
                        </td> 
                    </tr>
                </table>           
                <br />            
                <table width="98%" class="estilo_fuente">
                    <tr>
                        <td width="100%">
                            <table width="100%" class="estilo_fuente">
                                <tr>
                                    <td>
                                        <asp:Panel id="Pnl_Nomina_Periodo" runat="server" GroupingText="Calendario Nomina">
                                            <table width="100%">
                                                <tr>
                                                    <td style="width:13%;text-align:left;">Nomina</td>
                                                    <td style="width:20%;text-align:left;">
                                                        <asp:TextBox ID="Txt_Nomina" runat="server" Width="95%" ReadOnly="true" BorderStyle="Solid" BorderWidth="1"></asp:TextBox>
                                                    </td>
                                                    <td style="width:13%;text-align:left;">Fecha Inicio</td>
                                                    <td style="width:20%;text-align:left;">
                                                        <asp:TextBox ID="Txt_Fecha_Inicio_Nomina" runat="server" Width="95%" ReadOnly="true" BorderStyle="Solid" BorderWidth="1"></asp:TextBox>
                                                    </td>
                                                    <td style="width:13%;text-align:left;">Fecha Termino</td>
                                                    <td style="width:20%;text-align:left;">
                                                        <asp:TextBox ID="Txt_Fecha_Termino_Nomina" runat="server" Width="95%" ReadOnly="true" BorderStyle="Solid" BorderWidth="1"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>            
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">                                      
                    <tr align="center">
                        <td>
                            <asp:GridView ID="Grid_Calendario_Reloj_Checador" runat="server" CssClass="GridView_1"
                                AutoGenerateColumns="False" GridLines="None" Width="100%" HeaderStyle-CssClass="tblHead"
                                onrowdatabound="Grid_Calendario_Reloj_Checador_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="Nomina_ID" HeaderText="Nomina ID" Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="No_Nomina" HeaderText="No" Visible="True">
                                        <HeaderStyle HorizontalAlign="Center" Width="6%" />
                                        <ItemStyle HorizontalAlign="Center" Width="6%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Fecha_Inicio_Nomina" HeaderText="Inicio Nominal" Visible="True"
                                        DataFormatString="{0:dd/MMM/yyyy}">
                                        <HeaderStyle HorizontalAlign="Center" Width="19%" />
                                        <ItemStyle HorizontalAlign="Center" Width="19%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Fecha_Fin_Nomina" HeaderText="Fin Nominal" Visible="True" DataFormatString="{0:dd/MMM/yyyy}">
                                        <HeaderStyle HorizontalAlign="Center" Width="19%" />
                                        <ItemStyle HorizontalAlign="Center" Width="19%" />
                                    </asp:BoundField>                                    
                                    <asp:TemplateField HeaderText="*Inicio Reloj">
                                    <ItemTemplate>
                                            <asp:TextBox ID="Txt_Fecha_Inicio_Reloj_Checador" runat="server" TabIndex="4" Width="70%"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Fecha_Inicio_Reloj_Checador" runat="server" 
                                                FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                                TargetControlID="Txt_Fecha_Inicio_Reloj_Checador" ValidChars="/_" />
                                            <cc1:CalendarExtender ID="DTP_Fecha_Inicio_Reloj_Checador" runat="server" 
                                                Format="dd/MMM/yyyy" OnClientShown="calendarShown"
                                                PopupButtonID="Btn_Fecha_Inicio_Reloj_Checador" 
                                                TargetControlID="Txt_Fecha_Inicio_Reloj_Checador" />
                                            <asp:ImageButton ID="Btn_Fecha_Inicio_Reloj_Checador" runat="server" 
                                                ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" 
                                                ToolTip="Seleccione la Fecha de Inicio" TabIndex="5" />
                                            <cc1:MaskedEditExtender ID="MEE_Fecha_Inicio_Reloj_Checador" runat="server" 
                                                ClearMaskOnLostFocus="false" Enabled="True" Filtered="/" Mask="99/LLL/9999" 
                                                MaskType="None" TargetControlID="Txt_Fecha_Inicio_Reloj_Checador" 
                                                UserDateFormat="DayMonthYear" UserTimeFormat="None" />                          
                                        </ItemTemplate>
                                        <HeaderStyle Width="22%" />
                                        <ItemStyle Width="22%" HorizontalAlign="Center"/>
                                   </asp:TemplateField>
                                   <asp:TemplateField HeaderText="*Fin Reloj">
                                        <ItemTemplate>
                                            <asp:TextBox ID="Txt_Fecha_Termino_Reloj_Checador" runat="server" TabIndex="6" Width="70%"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Fecha_Termino_Reloj_Checador" 
                                                runat="server" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                                TargetControlID="Txt_Fecha_Termino_Reloj_Checador" ValidChars="/_" />
                                            <cc1:CalendarExtender ID="DTP_Fecha_Termino_Reloj_Checador" runat="server" 
                                                Format="dd/MMM/yyyy" OnClientShown="calendarShown" 
                                                PopupButtonID="Btn_Fecha_Termino_Reloj_Checador" 
                                                TargetControlID="Txt_Fecha_Termino_Reloj_Checador" />
                                            <asp:ImageButton ID="Btn_Fecha_Termino_Reloj_Checador" runat="server" 
                                                ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" 
                                                ToolTip="Seleccione la Fecha de Termino" TabIndex="7" />
                                            <cc1:MaskedEditExtender ID="MEE_Fecha_Termino_Reloj_Checador" runat="server" 
                                                ClearMaskOnLostFocus="false" Enabled="True" Filtered="/" Mask="99/LLL/9999" 
                                                MaskType="None" TargetControlID="Txt_Fecha_Termino_Reloj_Checador" 
                                                UserDateFormat="DayMonthYear" UserTimeFormat="None" />                   
                                        </ItemTemplate>
                                        <HeaderStyle Width="22%" />
                                        <ItemStyle Width="22%" HorizontalAlign="Center" />
                                   </asp:TemplateField>
                                    <asp:BoundField DataField="Estatus" HeaderText="Estatus" Visible="True">
                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                                    </asp:BoundField>
                                </Columns>
                                <SelectedRowStyle CssClass="GridSelected" />
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
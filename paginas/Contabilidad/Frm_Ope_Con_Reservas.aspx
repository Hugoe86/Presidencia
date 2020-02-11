<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Con_Reservas.aspx.cs" Inherits="paginas_Contabilidad_Frm_Ope_Con_Reservas" Title="Untitled Page" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager_Polizas" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server" >
        <ContentTemplate>        
            <asp:UpdateProgress ID="Uprg_Polizas" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            
            <div id="Div_Compromisos" style="background-color:#ffffff; width:98%; height:100%;">
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo">Reservas</td>
                    </tr>
                    <tr>
                        <td aling="left">&nbsp;
                            <asp:UpdatePanel ID="Upnl_Mensajes_Error" runat="server" UpdateMode="Always" RenderMode="Inline">
                                <ContentTemplate>                         
                                    <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                                    <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"/>
                                </ContentTemplate>                                
                            </asp:UpdatePanel>                                      
                        </td>
                    </tr> 
                </table>

                <table width="98%"  border="0" cellspacing="0">
                    <tr align="center">
                        <td>                
                            <div align="right" class="barra_busqueda">                        
                                <table style="width:100%;height:28px;">
                                    <tr>
                                        <td align="left" style="width:59%;"> 
                                            <asp:UpdatePanel ID="Upnl_Botones_Operacion" runat="server" UpdateMode="Conditional" RenderMode="Inline" >
                                                <ContentTemplate> 
                                                    <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" 
                                                        CssClass="Img_Button" TabIndex="1"
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" 
                                                        onclick="Btn_Nuevo_Click" />
                                                    <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" 
                                                        CssClass="Img_Button" TabIndex="2"
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" 
                                                        onclick="Btn_Modificar_Click" OnClientClick="return confirm('¿Esta seguro de Cancelar la Reserva?');"   />
                                                    <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" 
                                                        CssClass="Img_Button" TabIndex="4"
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                                        onclick="Btn_Salir_Click" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>                                                
                                        </td>
                                        <td align="right" style="width:41%;">                                  
                                        </td>       
                                    </tr>         
                                </table>                      
                            </div>
                        </td>
                    </tr>
                </table>           
                 <div id="Div_Reservas_Presentacion" runat="server" style=" display:block" >
                            <table width ="98%" class="estilo_fuente">
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style=" width:15%">
                                            <asp:Label ID="Lbl_Unidad_Responsable_busqueda" runat="server" Text="Unidad Responsable"></asp:Label>
                                        </td>
                                        <td colspan="5">
                                            <asp:DropDownList ID="Cmb_Unidad_Responsable_Busqueda" runat="server" Width="99%">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Lbl_Fecha_Inicio" runat="server" Text="Fecha Inicio"></asp:Label>
                                        </td>
                                        <td style=" width:15%">
                                            <asp:TextBox ID="Txt_Fecha_Inicio" runat="server" Width="70%" TabIndex="6" MaxLength="11" Height="18px" />
                                        <cc1:TextBoxWatermarkExtender ID="TBWE_Txt_Fecha_Inicio" runat="server" 
                                            TargetControlID="Txt_Fecha_Inicio" WatermarkCssClass="watermarked" 
                                            WatermarkText="Dia/Mes/Año" Enabled="True" />
                                        <cc1:CalendarExtender ID="CE_Txt_Fecha_Inicio" runat="server" 
                                            TargetControlID="Txt_Fecha_Inicio" Format="dd/MMM/yyyy" Enabled="True" PopupButtonID="Btn_Fecha_inicio"/>
                                         <asp:ImageButton ID="Btn_Fecha_Inicio" runat="server"
                                            ImageUrl="../imagenes/paginas/SmallCalendar.gif" style="vertical-align:top;"
                                            Height="18px" CausesValidation="false"/>           
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
                                            ID="Mev_Txt_Fecha_Poliza" 
                                            runat="server" 
                                            ControlToValidate="Txt_Fecha_Inicio"
                                            ControlExtender="Mee_Txt_Fecha_Inicio" 
                                            EmptyValueMessage="Fecha Requerida"
                                            InvalidValueMessage="Fecha Inicio Invalida" 
                                            IsValidEmpty="false" 
                                            TooltipMessage="Ingrese o Seleccione la Fecha de Póliza"
                                            Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>  
                                        </td>
                                        <td style="width:10%">
                                            <asp:Label ID="Lbl_Fecha_Final" runat="server" Text="Fecha Final"></asp:Label>
                                        </td>
                                        <td style="width:15%">
                                            <asp:TextBox ID="Txt_Fecha_Final" runat="server" Width="70%" TabIndex="6" MaxLength="11" Height="18px" />
                                        <cc1:TextBoxWatermarkExtender ID="TBWE_Txt_Fecha_Final" runat="server" 
                                            TargetControlID="Txt_Fecha_Final" WatermarkCssClass="watermarked" 
                                            WatermarkText="Dia/Mes/Año" Enabled="True" />
                                        <cc1:CalendarExtender ID="CE_Txt_Fecha_Final" runat="server" 
                                            TargetControlID="Txt_Fecha_Final" Format="dd/MMM/yyyy" Enabled="True" PopupButtonID="Btn_Fecha_Final"/>
                                         <asp:ImageButton ID="Btn_Fecha_Final" runat="server"
                                            ImageUrl="../imagenes/paginas/SmallCalendar.gif" style="vertical-align:top;"
                                            Height="18px" CausesValidation="false"/>           
                                        <cc1:MaskedEditExtender 
                                            ID="Mee_Txt_Fecha_Final" 
                                            Mask="99/LLL/9999" 
                                            runat="server"
                                            MaskType="None" 
                                            UserDateFormat="DayMonthYear" 
                                            UserTimeFormat="None" Filtered="/"
                                            TargetControlID="Txt_Fecha_Final" 
                                            Enabled="True" 
                                            ClearMaskOnLostFocus="false"/>  
                                        <cc1:MaskedEditValidator 
                                            ID="MaskedEditValidator1" 
                                            runat="server" 
                                            ControlToValidate="Txt_Fecha_Final"
                                            ControlExtender="Mee_Txt_Fecha_Final" 
                                            EmptyValueMessage="Fecha Requerida"
                                            InvalidValueMessage="Fecha Final Invalida" 
                                            IsValidEmpty="false" 
                                            TooltipMessage="Ingrese o Seleccione la Fecha de Póliza"
                                            Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>  
                                        </td>
                                        <td style=" width:10%">
                                        &nbsp;&nbsp;
                                            <asp:Label ID="Lbl_Folio" runat="server" Text="No. Reserva"></asp:Label>
                                        </td>
                                        <td style=" width:10%">                                        
                                            <asp:TextBox ID="Txt_No_Folio" runat="server" Width="95%"></asp:TextBox>
                                        </td>
                                        <td style="width:10%">
                                            <asp:ImageButton ID="Btn_Buscar_Reserva" runat="server" 
                                                ToolTip="Consultar" TabIndex="6" onclick="Btn_Buscar_Reserva_Click"
                                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                                />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="7">
                                        <div style=" overflow:auto; height:200px;">
                                         <asp:GridView ID="Grid_Reservas" runat="server" CssClass="GridView_1" 
                                            AutoGenerateColumns="False"  GridLines="None" Width="98%" AutoPostBack="true"
                                             OnPageIndexChanging="Grid_Reservas_PageIndexChanging"
                                            HeaderStyle-CssClass="tblHead" OnSorting="Grid_Reservas_Sorting">
                                            <Columns>         
                                               <%-- <asp:ButtonField ButtonType="Image" CommandName="Select" OnClick="Btn_Seleccionar_Reserva_Click" 
                                                    ImageUrl="~/paginas/imagenes/gridview/blue_button.png" CommandArgument='<%# Eval("No_Reserva") %>' >
                                                    <ItemStyle Width="5%" />
                                                </asp:ButtonField>--%>
                                                <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="Btn_Seleccionar_Reserva" runat="server" ImageUrl="~/paginas/imagenes/gridview/blue_button.png"
                                                        OnClick="Btn_Seleccionar_Reserva_Click" CommandArgument='<%# Eval("No_Reserva") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" Width="3%" />
                                                <ItemStyle HorizontalAlign="Center" Width="3%" />
                                                </asp:TemplateField>                       
                                                <asp:BoundField DataField="No_Reserva" HeaderText="Reserva">
                                                    <HeaderStyle HorizontalAlign="Left" Width="5%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="5%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Dependencia_ID" HeaderText="Dependencia">
                                                    <HeaderStyle HorizontalAlign="Left" Width="2%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="2%" />
                                                </asp:BoundField> 
                                                <asp:BoundField DataField="Nombre_Dependencia" HeaderText="Dependencia">
                                                    <HeaderStyle HorizontalAlign="Left" Width="30%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="30%" />
                                                </asp:BoundField> 
                                                <asp:BoundField DataField="Concepto" HeaderText="Concepto">
                                                    <HeaderStyle HorizontalAlign="Left" Width="30%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="30%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MMM/yyyy}">
                                                    <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                    <ItemStyle HorizontalAlign="left" Width="15%" />
                                                </asp:BoundField>                                   
                                                <asp:BoundField DataField="Importe_Inicial" HeaderText="Importe_Inicial" DataFormatString="{0:c}">
                                                    <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                    <ItemStyle HorizontalAlign="left" Width="10%" />
                                                </asp:BoundField> 
                                                 <asp:BoundField DataField="Beneficiario" HeaderText="Beneficiario">
                                                    <HeaderStyle HorizontalAlign="Left" Width="1%" />
                                                    <ItemStyle HorizontalAlign="left" Width="1%" />
                                                </asp:BoundField> 
                                                <asp:BoundField DataField="Estatus" HeaderText="Estatus">
                                                    <HeaderStyle HorizontalAlign="Center"  Width="5%" />
                                                    <ItemStyle HorizontalAlign="Center"  Width="5%" />
                                                </asp:BoundField>
                                            </Columns>
                                            <SelectedRowStyle CssClass="GridSelected" />
                                            <PagerStyle CssClass="GridHeader" />
                                            <HeaderStyle CssClass="tblHead" />
                                            <AlternatingRowStyle CssClass="GridAltItem" />
                                            </asp:GridView>   
                                        </div>                                                 
                                        </td>
                                    </tr>
                            </table>
                        </div>
                        <div id="Div_Reserva_Datos" runat="server"  style=" display:block" >
                        <asp:Panel ID="Pnl_Datos_Generales" runat="server" GroupingText="Datos de Reserva"
                        Width="100%">
                            <table width="98%">
                                <tr>
                                    <td colspan="4">
                                          &nbsp;                                  
                                    </td>
                                </tr>
                                <tr>
                                    <td style=" width:25%">
                                    &nbsp;
                                    &nbsp;
                                        <asp:Label ID="Lbl_No_Reserva" runat="server" Text="No. Reserva"></asp:Label> 
                                    </td>
                                    <td style=" width:30%">
                                        <asp:TextBox ID="Txt_No_Reserva" runat="server" ReadOnly ="true" Width="90%"></asp:TextBox>
                                    </td>
                                    <td style=" width:10%">
                                    &nbsp;
                                        <asp:Label ID="Lbl_Fecha" runat="server" Text="Fecha"></asp:Label> 
                                    </td>
                                    <td style=" width:35%">
                                        <asp:TextBox ID="Txt_Fecha" runat="server" Enabled="False" Width="56%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    &nbsp;
                                    &nbsp;
                                        <asp:Label ID="Lbl_Unidad_Responsable" runat="server" Text="*Unidad Responsable"></asp:Label></td>
                                    <td colspan="3" style=" width:60%">
                                        <asp:DropDownList ID="Cmb_Unidad_Responsable" runat="server" Width="80%">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    &nbsp;
                                    &nbsp;
                                        <asp:Label ID="Lbl_Fuente_Financiamiento" runat="server" Text="*Fuente Financiamiento"></asp:Label></td>
                                    <td colspan="3" style=" width:60%">
                                        <asp:DropDownList ID="Cmb_Fuente_Financiamiento" runat="server" Width="80%">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    &nbsp;
                                    &nbsp;
                                        <asp:Label ID="Lbl_Programa" runat="server" Text="*Programa"></asp:Label></td>
                                    <td colspan="3" style=" width:60%">
                                        <asp:DropDownList ID="Cmb_Programa" runat="server" Width="80%"  OnSelectedIndexChanged="Cmb_Programa_SelectedIndexChanged"
                                        AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    &nbsp;
                                    &nbsp;
                                        <asp:Label ID="Lbl_Partida" runat="server" Text="*Partida"></asp:Label></td>
                                    <td colspan="3" style=" width:60%">
                                        <asp:DropDownList ID="Cmb_Partida" runat="server" Width="80%" OnSelectedIndexChanged="Cmb_Partida_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                 <tr>
                                    <td>
                                    &nbsp;
                                    &nbsp;
                                        <asp:Label ID="Lbl_Estatus" runat="server" Text="*Estatus"></asp:Label></td><td>
                                        <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="90%" >
                                        <asp:ListItem Value="GENERADA">GENERADA</asp:ListItem>
                                        <asp:ListItem Value="CANCELADA">CANCELADA</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="2">
                                    </td> 
                                </tr>
                                <tr>
                                    <td>
                                    &nbsp;
                                    &nbsp;
                                        <asp:Label ID="Lbl_Beneficiario" runat="server" Text="Beneficiario"></asp:Label></td>
                                    <td colspan="3" style=" width:60%">
                                            <asp:TextBox ID="Txt_Beneficiario" runat="server" MaxLength="200" Width="80%"></asp:TextBox>
                                    </td>
                                </tr>
                                 <tr>
                                    <td style=" width:20%">
                                    &nbsp;
                                    &nbsp;
                                        <asp:Label ID="Lbl_Importe" runat="server" Text="*Importe"></asp:Label> 
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txt_Importe" runat="server" Width="90%" onblur=" $('input[id$=Txt_Importe]').formatCurrency({colorize:true, region: 'es-MX'});"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender  ID="FTE_Txt__Importe" runat="server" FilterType="Numbers,Custom" ValidChars="."  TargetControlID="Txt_Importe" ></cc1:FilteredTextBoxExtender>
                                        
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="Lbl_disponible" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                 <tr runat="server" id="tr_Saldo">
                                    <td style=" width:20%">
                                    &nbsp;
                                    &nbsp;
                                        <asp:Label ID="Lbl_Saldo" runat="server" Text="Saldo"></asp:Label> 
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txt_Saldo" runat="server" Width="90%" ></asp:TextBox>                                       
                                    </td>
                                    <td colspan="2">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    &nbsp;
                                    &nbsp;
                                        <asp:Label ID="Lbl_Concepto" runat="server" Text="*Concepto"></asp:Label></td>
                                    <td colspan="3" style=" width:60%">
                                        <asp:TextBox ID="Txt_Conceptos" runat="server" TextMode="MultiLine" Width="80%"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        </div>
              </div>
            </ContentTemplate>
      </asp:UpdatePanel>
</asp:Content>
<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Con_Cheques.aspx.cs" Inherits="paginas_Contabilidad_Frm_Ope_Con_Cheques" Title="Untitled Page" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
<script src="../../easyui/jquery-1.4.2.js" type="text/javascript"></script>
<script type="text/javascript">
    $(document).ready(function() {
        $("select[id$=Cmb_Tipo_Pago]").change(function() {
            $(".Cmb_Tipo_Pago option:selected").each(function() {
                var Tipo = $(this).val();
                if (Tipo == "CHEQUE") {
                    $("#Tr_Cheques").show();
                    $("#Tr_Referencia").hide();
                    document.getElementById("<%=Txt_Referencia_Pago.ClientID%>").value = ""
                }
                if (Tipo == "TRANSFERENCIA") {
                    $("#Tr_Cheques").hide();
                    $("#Tr_Referencia").show();
                    document.getElementById("<%=Txt_No_Cheque.ClientID%>").value = ""
                }
            });
        }).trigger('change');
        $("select[id$=Cmb_Estatus]").change(function() {
        $(".Estatus option:selected").each(function() {
                var Tipo = $(this).val();
                if (Tipo == "PAGADO") {
                    $("#Tr_Estatus").hide();
                    document.getElementById("<%=Txt_Motivo_Cancelacion.ClientID%>").value = ""
                }
                if (Tipo == "CANCELADO") {
                    $("#Tr_Estatus").show();
                    document.getElementById("<%=Txt_Motivo_Cancelacion.ClientID%>").value = ""
                }
            });
        }).trigger('change');
    });
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager_Polizas" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server" >
        <ContentTemplate>        
           <%-- <asp:UpdateProgress ID="Uprg_Polizas" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>--%>
            
            <div id="Div_Compromisos" style="background-color:#ffffff; width:98%; height:100%;">
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo">Cheques</td>
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
                                                        CssClass="Img_Button" TabIndex="2"  OnClientClick="return confirm('¿Esta Seguro de Cancelar el Pago?');" 
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" 
                                                        onclick="Btn_Modificar_Click" /> 
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
                 <div id="Div_Solicitudes_Pendientes" runat="server" style="display:block" >
                            <table width ="98%" class="estilo_fuente">
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        &nbsp;&nbsp;
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
                                    </tr>
                                    <tr>
                                        <td style=" width:10%">
                                        &nbsp;&nbsp;
                                            <asp:Label ID="Lbl_Folio" runat="server" Text="No. Solicitud_Pago"></asp:Label>
                                        </td>
                                        <td style=" width:10%">                                        
                                            <asp:TextBox ID="Txt_No_Solicitud_Pago" runat="server" Width="70%"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender  ID="FilteredTextBoxExtender1" runat="server" FilterType ="Numbers" TargetControlID="Txt_No_Solicitud_Pago" ></cc1:FilteredTextBoxExtender>
                                        </td>
                                        <td style=" width:10%">
                                            <asp:Label ID="Lbl_Tipo_solicitud" runat="server" Text="Tipo_Solicitud"></asp:Label>
                                        </td>
                                        <td style=" width:10%">
                                            <asp:DropDownList ID="Cmb_Tipo_Solicitud" runat="server"  Width="70%">
                                            </asp:DropDownList> 
                                        </td>
                                        <td style="width:10%">
                                            <asp:ImageButton ID="Btn_Buscar_Solicitud" runat="server" 
                                                ToolTip="Consultar" TabIndex="6" 
                                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" onclick="Btn_Buscar_Solicitud_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                            <asp:HiddenField ID="Txt_Cuenta_Contable_ID_Banco" runat="server" />
                                            <asp:HiddenField ID="Txt_Cuenta_Contable_Proveedor" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5">
                                        <div style="overflow:auto; height:200px;">
                                            <asp:GridView ID="Grid_Solicitudes" runat="server"   CssClass="GridView_1"
                                            AutoGenerateColumns="False" GridLines="None" Width="98%" AutoPostBack="true"
                                            HeaderStyle-CssClass="tblHead"> <%--AllowPaging="True"  PageSize="5"--%> <%--OnPageIndexChanging="Grid_Solicitudes_PageIndexChanging"--%>
                                            <Columns>         
                                               <%-- <asp:ButtonField ButtonType="Image" CommandName="Select" OnClick="Btn_Seleccionar_Reserva_Click" 
                                                    ImageUrl="~/paginas/imagenes/gridview/blue_button.png" CommandArgument='<%# Eval("No_Reserva") %>' >
                                                    <ItemStyle Width="5%" />
                                                </asp:ButtonField>--%>
                                                <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="Btn_Seleccionar_Solicitudes" runat="server" ImageUrl="~/paginas/imagenes/gridview/blue_button.png"
                                                         CommandArgument='<%# Eval("No_Solicitud_Pago") %>' OnClick="Btn_Seleccionar_Solicitud_Click" />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" Width="3%" />
                                                <ItemStyle HorizontalAlign="Center" Width="3%" />
                                                </asp:TemplateField>                       
                                                <asp:BoundField DataField="No_Solicitud_Pago" HeaderText="No_Solicitud_Pago">
                                                    <HeaderStyle HorizontalAlign="Left" Width="5%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="5%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Tipo_Solicitud_Pago_Id" HeaderText="Tipo_Solicitud_Pago_Id">
                                                    <HeaderStyle HorizontalAlign="Left" Width="5%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="5%" />
                                                </asp:BoundField> 
                                                <asp:BoundField DataField="Tipo_Pago" HeaderText="Tipo_Pago">
                                                    <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                </asp:BoundField> 
                                                <asp:BoundField DataField="Concepto" HeaderText="Concepto">
                                                    <HeaderStyle HorizontalAlign="Left" Width="40%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="40%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Fecha_Solicitud" HeaderText="Fecha_Solicitud" DataFormatString="{0:dd/MMM/yyyy}">
                                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                    <ItemStyle HorizontalAlign="left" Width="20%" />
                                                </asp:BoundField>                                   
                                                <asp:BoundField DataField="Monto" HeaderText="Monto" DataFormatString="{0:c}">
                                                    <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                    <ItemStyle HorizontalAlign="left" Width="10%" />
                                                </asp:BoundField> 
                                                 <asp:BoundField DataField="Estatus" HeaderText="Estatus">
                                                    <HeaderStyle HorizontalAlign="Left" Width="1%" />
                                                    <ItemStyle HorizontalAlign="left" Width="1%" />
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
                        <div id="Div_Datos_Solicitud" runat="server"  style=" display:block" >
                        <asp:Panel ID="Pnl_Datos_Generales" runat="server" GroupingText="Datos de la Solicitud"
                        Width="98%">
                            <table width="98%" class="estilo_fuente">
                                <tr>
                                    <td colspan="4">
                                          &nbsp;                                  
                                    </td>
                                </tr>
                                <tr>
                                    <td style=" width:25%">
                                    &nbsp;
                                    &nbsp;
                                        <asp:Label ID="Lbl_No_Solicitud" runat="server" Text="No. Solicitud"></asp:Label> 
                                    </td>
                                    <td style=" width:30%">
                                        <asp:TextBox ID="Txt_No_Solicitud" runat="server" ReadOnly ="true" Width="90%"></asp:TextBox>
                                    </td>
                                    <td style=" width:10%">
                                    &nbsp;
                                        <asp:Label ID="Lbl_Fecha" runat="server" Text="Fecha Solicitud"></asp:Label> 
                                    </td>
                                    <td style=" width:35%">
                                        <asp:TextBox ID="Txt_Fecha" runat="server" Enabled="False" Width="56%"></asp:TextBox>
                                    </td>
                                </tr>
                                 <tr>
                                    <td style=" width:25%">
                                    &nbsp;
                                    &nbsp;
                                        <asp:Label ID="Lbl_Tipo_Solicitud_pago" runat="server" Text="Tipo Solicitud"></asp:Label> 
                                    </td>
                                    <td style=" width:30%">
                                        <asp:TextBox ID="Txt_Tipo_Solicitud_Pago" runat="server" ReadOnly ="true" Width="90%"></asp:TextBox>
                                    </td>
                                    <td style=" width:10%">
                                    &nbsp;
                                        <asp:Label ID="Lbl_Mes_Anio" runat="server" Text="Mes/Año"></asp:Label> 
                                    </td>
                                    <td style=" width:35%">
                                        <asp:TextBox ID="Txt_MesAnio" runat="server" Enabled="False" Width="56%"></asp:TextBox>
                                    </td>
                                </tr>
                                 <tr>
                                    <td style=" width:25%">
                                    &nbsp;
                                    &nbsp;
                                        <asp:Label ID="Lbl_reserva" runat="server" Text="No. Reserva"></asp:Label> 
                                    </td>
                                    <td style=" width:30%">
                                        <asp:TextBox ID="Txt_No_Reserva_Solicitud" runat="server" ReadOnly ="true" Width="90%"></asp:TextBox>
                                    </td>
                                    <td style=" width:10%">
                                    &nbsp;
                                        <asp:Label ID="Lbl_Estatus" runat="server" Text="Estatus"></asp:Label> 
                                    </td>
                                    <td style=" width:35%">
                                        <asp:TextBox ID="Txt_Estatus_Solicitud" runat="server" Enabled="False" Width="56%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    &nbsp;
                                    &nbsp;
                                        <asp:Label ID="Lbl_Concepto" runat="server" Text="Concepto"></asp:Label></td>
                                    <td colspan="3" style=" width:60%">
                                            <asp:TextBox ID="Txt_Concepto" runat="server" MaxLength="200" Width="80%"></asp:TextBox>
                                    </td>
                                </tr>
                                 <tr>
                                    <td style=" width:25%">
                                    &nbsp;
                                    &nbsp;
                                        <asp:Label ID="Lbl_Monto" runat="server" Text="Monto"></asp:Label> 
                                    </td>
                                    <td style=" width:30%">
                                        <asp:TextBox ID="Txt_Monto" runat="server" ReadOnly ="true" Width="90%"></asp:TextBox>
                                    </td>
                                    <td style=" width:10%">
                                    &nbsp;
                                        <asp:Label ID="Lbl_No_Poliza" runat="server" Text="No. Poliza"></asp:Label> 
                                    </td>
                                    <td style=" width:35%">
                                        <asp:TextBox ID="Txt_No_Poliza" runat="server" Enabled="False" Width="56%"></asp:TextBox>
                                    </td>
                                </tr>
                                 <tr>
                                    <td style=" width:25%">
                                    &nbsp;
                                    &nbsp;
                                        <asp:Label ID="Lbl_No_Factura" runat="server" Text="No. Factura"></asp:Label> 
                                    </td>
                                    <td style=" width:30%">
                                        <asp:TextBox ID="Txt_No_Factura" runat="server" ReadOnly ="true" Width="90%"></asp:TextBox>
                                    </td>
                                    <td style=" width:10%">
                                    &nbsp;
                                        <asp:Label ID="Lbl_Fecha_Factura" runat="server" Text="Fecha Factura"></asp:Label> 
                                    </td>
                                    <td style=" width:35%">
                                        <asp:TextBox ID="Txt_Fecha_Factura" runat="server" Enabled="False" Width="56%"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                            <asp:Panel ID="Pnl_Datos_Pago" runat="server" GroupingText="Datos de Pago" 
                                Width="98%">
                                <table class="estilo_fuente" width="98%">
                                    <tr>
                                        <td colspan="4">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style=" width:25%">
                                            &nbsp; &nbsp;
                                            <asp:Label ID="Lbl_No_Pago" runat="server" Text="No. Pago"></asp:Label>
                                        </td>
                                        <td style=" width:30%">
                                            <asp:TextBox ID="Txt_No_Pago" runat="server" ReadOnly="true" Width="90%"></asp:TextBox>
                                        </td>
                                        <td style=" width:10%">
                                            &nbsp;
                                            <asp:Label ID="Lbl_Fecha_No_Pago" runat="server" Text="Fecha Pago" ></asp:Label>
                                        </td>
                                        <td style=" width:35%">
                                            <asp:TextBox ID="Txt_Fecha_No_Pago" runat="server" ReadOnly="true" Width="56%"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style=" width:25%">
                                            &nbsp; &nbsp;
                                            <asp:Label ID="Lbl_Tipo_Pago" runat="server" Text="Tipo_Pago"></asp:Label>
                                        </td>
                                        <td style=" width:30%">
                                            <asp:DropDownList ID="Cmb_Tipo_Pago" runat="server" CssClass="Cmb_Tipo_Pago" Width="90%">
                                                <asp:ListItem Value="CHEQUE">CHEQUE</asp:ListItem>
                                                <asp:ListItem Value="TRANSFERENCIA">TRANSFERENCIA</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                         <td style=" width:10%">
                                            &nbsp;
                                            <asp:Label ID="Lbl_Estatus_Pago" runat="server" Text="Estatus"></asp:Label>
                                        </td>
                                        <td style=" width:35%">
                                            <asp:DropDownList ID="Cmb_Estatus" runat="server" CssClass="Estatus" width="56%" >
                                            <asp:ListItem Value="PAGADO">PAGADO</asp:ListItem>
                                            <asp:ListItem Value="CANCELADO">CANCELADO</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr style="display:none;" id="Tr_Estatus" >
                                        <td>
                                            &nbsp; &nbsp;   
                                            <asp:Label ID="Lbl_Motivo_Cancelacion" runat="server" Text="*Motivo Cancelación" ></asp:Label>
                                        </td>
                                         <td colspan="3" style=" width:60%">
                                         <asp:TextBox ID="Txt_Motivo_Cancelacion" runat="server"  Width="80%" MaxLength ="250"  ></asp:TextBox>
                                            </td>
                                    </tr>
                                     <tr>
                                        <td style=" width:25%">
                                            &nbsp; &nbsp;
                                            <asp:Label ID="Lbl_banco" runat="server" Text="Banco"></asp:Label>
                                        </td>
                                        <td style=" width:30%">
                                            <asp:DropDownList ID="Cmb_Banco" runat="server" Width="90%">
                                            </asp:DropDownList>
                                        </td>
                                        <td style=" width:10%">
                                        </td>
                                        <td style=" width:35%">
                                        </td>
                                    </tr>
                                    <tr style="display:block;" id="Tr_Cheques" >
                                        <td>
                                            &nbsp; &nbsp;   
                                            <asp:Label ID="Lbl_No_Cheque" runat="server" Text="*No_Cheque" ></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Txt_No_Cheque" runat="server"  Width="90%" MaxLength ="20"  ></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender  ID="FTE_Txt_Cheque" runat="server" FilterType ="Numbers" TargetControlID="Txt_No_Cheque" ></cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr style="display:none;" id="Tr_Referencia" >
                                        <td>
                                            &nbsp; &nbsp;   
                                            <asp:Label ID="Lbl_Referencia" runat="server" Text="*Referencia" ></asp:Label>
                                        </td>
                                        <td colspan="3" style=" width:60%">
                                            <asp:TextBox ID="Txt_Referencia_Pago" runat="server"  Width="80%" MaxLength ="150"  ></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp; &nbsp;   
                                            <asp:Label ID="Lbl_comentario" runat="server" Text="*Comentario" ></asp:Label>
                                        </td>
                                        <td colspan="3" style=" width:60%">
                                            <asp:TextBox ID="Txt_Comentario_Pago" runat="server"  Width="80%" MaxLength ="250"  ></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp; &nbsp;   
                                            <asp:Label ID="Lbl_Beneficiario" runat="server" Text="*Beneficiario_Pago" ></asp:Label>
                                        </td>
                                        <td colspan="3" style=" width:60%">
                                            <asp:TextBox ID="Txt_Beneficiario_Pago" runat="server"  Width="80%" MaxLength ="250"  ></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                  <script type="text/javascript" language="javascript">
                                      Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(iniciar_peticion);
                                      Sys.WebForms.PageRequestManager.getInstance().add_endRequest(fin_peticion);

                                      function fin_peticion() {
                                          $(document).ready(function() {
                                              $("select[id$=Cmb_Tipo_Pago]").change(function() {
                                              $(".Cmb_Tipo_Pago option:selected").each(function() {
                                                  var Tipo = $(this).val();
                                                  if (Tipo == "CHEQUE") {
                                                      $("#Tr_Cheques").show();
                                                      $("#Tr_Referencia").hide();
                                                      document.getElementById("<%=Txt_Referencia_Pago.ClientID%>").value = ""
                                                  }
                                                  if (Tipo == "TRANSFERENCIA") {
                                                      $("#Tr_Cheques").hide();
                                                      $("#Tr_Referencia").show();
                                                      document.getElementById("<%=Txt_No_Cheque.ClientID%>").value = ""
                                                  }
                                              });
                                          }).trigger('change');
                                          $("select[id$=Cmb_Estatus]").change(function() {
                                              $(".Estatus option:selected").each(function() {
                                                  var Tipo = $(this).val();
                                                  if (Tipo == "PAGADO") {
                                                      $("#Tr_Estatus").hide();
                                                      document.getElementById("<%=Txt_Motivo_Cancelacion.ClientID%>").value = ""
                                                  }
                                                  if (Tipo == "CANCELADO") {
                                                      $("#Tr_Estatus").show();
                                                      document.getElementById("<%=Txt_Motivo_Cancelacion.ClientID%>").value = ""
                                                  }
                                              });
                                          }).trigger('change');
                                          });
                                      }
                                      function iniciar_peticion() {
                                          $(document).ready(function() {
                                              $("select[id$=Cmb_Tipo_Pago]").change(function() {
                                              $(".Cmb_Tipo_Pago option:selected").each(function() {
                                                  var Tipo = $(this).val();
                                                  if (Tipo == "CHEQUE") {
                                                      $("#Tr_Cheques").show();
                                                      $("#Tr_Referencia").hide();
                                                      document.getElementById("<%=Txt_Referencia_Pago.ClientID%>").value = ""
                                                  }
                                                  if (Tipo == "TRANSFERENCIA") {
                                                      $("#Tr_Cheques").hide();
                                                      $("#Tr_Referencia").show();
                                                      document.getElementById("<%=Txt_No_Cheque.ClientID%>").value = ""
                                                  }
                                              });
                                          }).trigger('change');
                                          $("select[id$=Cmb_Estatus]").change(function() {
                                              $(".Estatus option:selected").each(function() {
                                                  var Tipo = $(this).val();
                                                  if (Tipo == "PAGADO") {
                                                      $("#Tr_Estatus").hide();
                                                      document.getElementById("<%=Txt_Motivo_Cancelacion.ClientID%>").value = ""
                                                  }
                                                  if (Tipo == "CANCELADO") {
                                                      $("#Tr_Estatus").show();
                                                      document.getElementById("<%=Txt_Motivo_Cancelacion.ClientID%>").value = ""
                                                  }
                                              });
                                          }).trigger('change');
                                          });
                                      }
</script>
                            </asp:Panel>                      
		                </div>
              </div>
            </ContentTemplate>
      </asp:UpdatePanel>
</asp:Content>
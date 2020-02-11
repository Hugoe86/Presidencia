<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Pre_Dias_Inhabiles.aspx.cs" Inherits="paginas_Predial_Ope_Pre_Dias_Inhabiles" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">

    <script type="text/javascript" language="javascript">
        //Metodo para mantener los calendarios en una capa mas alat.
        function calendarShown(sender, args)
        {
            sender._popupBehavior._element.style.zIndex = 10000005;
        } 
        function pageLoad() 
        {
            $('[id*=Txt_Motiv').keyup(function() {var Caracteres =  $(this).val().length;if (Caracteres > 250) {this.value = this.value.substring(0, 250);$(this).css("background-color", "Yellow");$(this).css("color", "Red");}else{$(this).css("background-color", "White");$(this).css("color", "Black");}$('#Contador_Caracteres_Motivo_Dia_Inhabil').text('Carácteres Ingresados [ ' + Caracteres + ' ]/[ 250 ]');});
        }
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <asp:ScriptManager ID="ScptM_Dias_Inhabiles" runat="server" EnableScriptGlobalization="true"
        EnableScriptLocalization="True" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                        <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                    </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Requisitos" style="background-color: #ffffff; width: 100%; height: 100%;">
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo">
                           Catálogo de Días Inhábiles
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png"
                                Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table width="98%" border="0" cellspacing="0">
                    <tr align="center">
                        <td>
                            <div align="right" class="barra_busqueda">
                                <table style="width: 100%; height: 28px;">
                                    <tr>
                                        <td align="left" style="width: 59%;">
                                            <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button"
                                                TabIndex="1" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" OnClick="Btn_Nuevo_Click" />
                                            <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button"
                                                TabIndex="2" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" OnClick="Btn_Modificar_Click" />
                                            <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar" CssClass="Img_Button"
                                                TabIndex="3" ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png" OnClientClick="return confirm('¿Está seguro de eliminar el Día seleccionado?');"
                                                OnClick="Btn_Eliminar_Click" />
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                                TabIndex="4" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Salir_Click" />
                                        </td>
                                        <td align="right" style="width: 41%;">
                                            <table style="width: 100%; height: 28px;">
                                                <tr>
                                                    <td style="vertical-align: middle; text-align: right; width: 20%;">
                                                        B&uacute;squeda:
                                                    </td>
                                                    <td style="width: 55%;">
                                                        <asp:TextBox ID="Txt_Busqueda_Año" runat="server" MaxLength="4" TabIndex="5" ToolTip="Buscar por Año"
                                                            Width="180px" />
                                                        <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Año" runat="server" WatermarkCssClass="watermarked"
                                                            WatermarkText="<Ingrese Nombre>" TargetControlID="Txt_Busqueda_Año" />
                                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Año" runat="server" TargetControlID="Txt_Busqueda_Año"
                                                            FilterType="Custom, Numbers" />
                                                    </td>
                                                    <td style="vertical-align: middle; width: 5%;">
                                                        <asp:ImageButton ID="Btn_Buscar_Año" runat="server" TabIndex="6" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                                            ToolTip="Consultar" OnClick="Btn_Buscar_Año_Click" />
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
                <br />
                <table width="98%" class="estilo_fuente">
                    <tr>
                        <td style="text-align: left; width: 15%;">
                            No Dia Inhabil
                        </td>
                        <td style="text-align: left; width: 35%;">
                            <asp:TextBox ID="Txt_No_Dia_Inhabil" runat="server" ReadOnly="True" Width="97%" BorderStyle="Solid"
                                BorderWidth="1" />
                        </td>
                        <td style="text-align: left; width: 15%;">
                            Estatus
                        </td>
                        <td style="text-align: left; width: 35%;">
                            <asp:DropDownList ID="Cmb_Estatus_Dia_Inhabil" runat="server" Width="97%" TabIndex="7">
                                <asp:ListItem>&lt;-Seleccione-&gt;</asp:ListItem>
                                <asp:ListItem>VIGENTE</asp:ListItem>
                                <asp:ListItem>BAJA</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 15%;">
                            *Descripcion
                        </td>
                        <td colspan="3" style="text-align: left; width: 85%;">
                            <asp:TextBox ID="Txt_Descripcion_Dia_Inhabil" runat="server" Width="98%" MaxLength="100"
                                TabIndex="8" style="text-transform:uppercase"/><%--
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Descripcion_Dia_Inhabil" runat="server"
                                TargetControlID="Txt_Descripcion_Dia_Inhabil" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers"
                                ValidChars="ÑñáéíóúÁÉÍÓÚ. " />--%>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 15%;">
                            *Tipo Dia
                        </td>
                        <td style="text-align: left; width: 35%;">
                            <asp:DropDownList ID="Cmb_Tipo_Dia_Inhabil" runat="server" Width="97%" TabIndex="9"
                                OnSelectedIndexChanged="Cmb_Tipo_Dia_Inhabil_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem>&lt;-Seleccione-&gt;</asp:ListItem>
                                <asp:ListItem>OTRO</asp:ListItem>
                                <asp:ListItem>DIA FESTIVO</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="text-align: left; width: 15%;">
                            Dia Festivo
                        </td>
                        <td style="text-align: left; width: 35%;">
                            <asp:DropDownList ID="Cmb_Dia_Festivo" runat="server" Width="97%" TabIndex="10" OnSelectedIndexChanged="Cmb_Dia_Festivo_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 15%;">
                            *Día Inhabil
                        </td>
                        <td style="text-align: left; width: 35%;">
                            <asp:TextBox ID="Txt_Fecha_Dia_Inhabil" runat="server" TabIndex="11" Width="87%"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Fecha_Dia_Inhabil" runat="server" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters"
                                TargetControlID="Txt_Fecha_Dia_Inhabil" ValidChars="/_" />
                            <cc1:CalendarExtender ID="DTP_Fecha_Dia_Inhabil" runat="server" Format="dd/MMM/yyyy"
                                OnClientShown="calendarShown" PopupButtonID="Btn_Fecha_Dia_Inhabil" TargetControlID="Txt_Fecha_Dia_Inhabil" />
                            <asp:ImageButton ID="Btn_Fecha_Dia_Inhabil" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                ToolTip="Seleccione la Fecha del Dia Inhabil" TabIndex="12" />
                            <cc1:MaskedEditExtender ID="MEE_Txt_Fecha_Dia_Inhabil" runat="server" ClearMaskOnLostFocus="false"
                                Enabled="True" Filtered="/" Mask="99/LLL/9999" MaskType="None" TargetControlID="Txt_Fecha_Dia_Inhabil"
                                UserDateFormat="DayMonthYear" UserTimeFormat="None" />
                        </td>
                        <td colspan="2" style="text-align: left; width: 50%;">
                        </td>
                        <%--<td style="text-align:left;width:15%;">*Dia Apliacion</td>
                        <td style="text-align:left;width:35%;">
                            <asp:TextBox ID="Txt_Fecha_Aplicacion" runat="server" TabIndex="13" Width="87%"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Fecha_Aplicacion" 
                                runat="server" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                TargetControlID="Txt_Fecha_Aplicacion" ValidChars="/_" />
                            <cc1:CalendarExtender ID="DTP_Fecha_Aplicacion" runat="server" 
                                Format="dd/MMM/yyyy" OnClientShown="calendarShown" 
                                PopupButtonID="Btn_Fecha_Aplicacion" 
                                TargetControlID="Txt_Fecha_Aplicacion" />
                            <asp:ImageButton ID="Btn_Fecha_Aplicacion" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" 
                                ToolTip="Seleccione la Fecha de Aplicacion" TabIndex="14"/>
                            <cc1:MaskedEditExtender ID="MEE_Fecha_Aplicacion" runat="server" 
                                ClearMaskOnLostFocus="false" Enabled="True" Filtered="/" Mask="99/LLL/9999" 
                                MaskType="None" TargetControlID="Txt_Fecha_Aplicacion" 
                                UserDateFormat="DayMonthYear" UserTimeFormat="None" />
                        </td>--%>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 15%;">
                            Motivo
                        </td>
                        <td colspan="3" style="text-align: left; width: 85%;">
                            <asp:TextBox ID="Txt_Motivo_Dia_Inhabil" runat="server" TabIndex="15" MaxLength="250"
                                TextMode="MultiLine" Width="98%" />
                            <span id="Contador_Caracteres_Motivo_Dia_Inhabil" class="watermarked"></span>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Motivo_Dia_Inhabil" runat="server" WatermarkCssClass="watermarked"
                                TargetControlID="Txt_Motivo_Dia_Inhabil" WatermarkText="Límite de Caractes 250" />
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Motivo_Dia_Inhabil" runat="server" TargetControlID="Txt_Motivo_Dia_Inhabil"
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " />
                        </td>
                    </tr>
                </table>
                <br />
                <table width="98%" border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td>
                            <asp:GridView ID="Grid_Dias_Inhabiles" runat="server" AllowPaging="True" CssClass="GridView_1"
                                AutoGenerateColumns="False" PageSize="5" GridLines="None" Width="100%" AllowSorting="True"
                                HeaderStyle-CssClass="tblHead" OnPageIndexChanging="Grid_Dias_Inhabiles_PageIndexChanging"
                                OnSelectedIndexChanged="Grid_Dias_Inhabiles_SelectedIndexChanged" OnSorting="Grid_Dias_Inhabiles_Sorting">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="No_Dia_Inhabil" HeaderText="No Dia Inhabil" Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Dia_ID" HeaderText="Dia ID" Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Fecha" HeaderText="Mes" Visible="True" DataFormatString="{0:MMM}"
                                        SortExpression="Fecha">
                                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Fecha" HeaderText="Dia Inhabil" Visible="True" DataFormatString="{0:dd/MMM/yyyy}"
                                        SortExpression="Fecha">
                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" Width="60%" />
                                        <ItemStyle HorizontalAlign="Left" Width="60%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Estatus" HeaderText="Estatus" Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Motivo" HeaderText="Motivo" Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
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

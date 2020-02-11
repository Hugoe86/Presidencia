<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Cat_Pre_Tabulador_Recargos.aspx.cs" Inherits="paginas_predial_Frm_Cat_Pre_Tabulador_Recargos"
    Title="Catalogo de Tabulador de Recargos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style1
        {
            width: 838px;
        }
        .style2
        {
            width: 5%;
        }
        .style4
        {
            width: 9%;
        }
        .style5
        {
            width: 17%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <asp:ScriptManager ID="ScptM_Tabulador_Recargos" runat="server" />
    <asp:UpdatePanel ID="Upd_Tabulador_Recargos" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Tabulador_Recargos"
                DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                    </div>
                    <div class="processMessage" id="div_progress">
                        <img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Tabulador_Recargos" style="background-color: #ffffff; width: 100%; height: 100%;">
                <table border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="2" class="label_titulo">
                            Cat&aacute;logo Tabulador de Recargos
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            <div id="Div_Contenedor_Msj_Error" style="width: 98%;" runat="server" visible="false">
                                <table style="width: 100%;">
                                    <tr>
                                        <td colspan="2" align="left">
                                            <asp:ImageButton ID="Img_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png"
                                                Width="24px" Height="24px" />
                                            <asp:Label ID="Lbl_Ecabezado_Mensaje" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 10%;">
                                        </td>
                                        <td style="width: 90%; text-align: left;" valign="top">
                                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr class="barra_busqueda" align="right">
                        <td align="left" class="style1">
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"
                                Width="24px" CssClass="Img_Button" AlternateText="Nuevo" OnClick="Btn_Nuevo_Click" />
                            <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                Width="24px" CssClass="Img_Button" AlternateText="Modificar" OnClick="Btn_Modificar_Click" />
                            <asp:ImageButton ID="Btn_Salir" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                Width="24px" CssClass="Img_Button" AlternateText="Salir" OnClick="Btn_Salir_Click" />
                        </td>
                        <td align="right" class="style1">
                            Búsqueda:
                            <asp:TextBox ID="Txt_Busqueda_Recargos" runat="server" Width="130px"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Buscar_Recargos" runat="server" CausesValidation="false"
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Buscar" OnClick="Btn_Buscar_Click" />
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Recargos" runat="server" WatermarkText="<Buscar por Año>"
                                TargetControlID="Txt_Busqueda_Recargos" WatermarkCssClass="watermarked" />
                            </TextBoxWatermarkExtender>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Recargos" runat="server" 
                                TargetControlID="Txt_Busqueda_Recargos" FilterType="Numbers" />
                            </FilteredTextBoxExtender>
                        </td>
                    </tr>
                </table>
                <br />
                <center>
                    <table width="98%" class="estilo_fuente" id="Tbl_Anio_Tabulador_Combo" runat="server" >
                        <tr>
                            <td style="text-align: right; width: 16%;">
                                <asp:Label ID="Lbl_Anio_Tabulador_Combo" runat="server" Text="* Año Tabulador">
                                </asp:Label>
                            </td>
                            <td style="text-align: right; width: 16%;">
                                <asp:DropDownList ID="Cmb_Anio_Tabulador" runat="server" Width="95%" OnSelectedIndexChanged="Cmb_Anio_Tabulador_SelectedIndexChanged"
                                    AutoPostBack="true" />
                            </td>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                    </table>
                        
                    <table width="98%" class="estilo_fuente" id="Tbl_Nuevo_Tabulador_Anio_bimestre" runat="server" >
                        <tr>
                            <td style="text-align: right; width: 10%;">
                                <asp:Label ID="Lbl_Anio_Tabulador_Texto" runat="server" Text="* Año Tabulador">
                                </asp:Label>
                            </td>
                            <td style="text-align: right; width: 16%;">
                                <asp:TextBox ID="Txt_Anio_Tabulador" runat="server" Width="95%" MaxLength="4" />
                                <cc1:FilteredTextBoxExtender ValidChars="1234567890" FilterType="Custom" ID="FilteredTextBoxExtender1"
                                    runat="server" TargetControlID="Txt_Anio_Tabulador">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                            <td style="text-align: right; width: 10%;">
                                <asp:Label ID="Lbl_Anio" runat="server" Text="* Año">
                                </asp:Label>
                            </td>
                            <td style="text-align: right; width: 16%;">
                                <asp:TextBox ID="Txt_Anio" runat="server" Width="95%" MaxLength="4" />
                                <cc1:FilteredTextBoxExtender ValidChars="1234567890" FilterType="Custom" ID="FTE_Txt_Anio"
                                    runat="server" TargetControlID="Txt_Anio">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                            <td style="text-align: right; width: 16%;">
                                <asp:Label ID="Lbl_Bimestre" runat="server" Text="* Bimestre">
                                </asp:Label>
                            </td>
                            <td style="text-align: right; width: 16%;">
                                <asp:TextBox ID="Txt_Bimestre" runat="server" Width="95%" MaxLength="1" />
                                <cc1:FilteredTextBoxExtender ValidChars="123456" FilterType="Custom" ID="FTE_Txt_Bimestre"
                                    runat="server" TargetControlID="Txt_Bimestre">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                    </table>
                        
                    <table width="98%" class="estilo_fuente" id="Tbl_Campos_Nuevo_Recargo" runat="server" >
                        <tr>
                            <td style="text-align: right; width: 16%;">
                                <asp:Label ID="Lbl_Enero" runat="server" Text="* Enero">
                                </asp:Label>
                            </td>
                            <td style="text-align: right; width: 16%;">
                                <asp:TextBox ID="Txt_Enero" runat="server" Width="95%" MaxLength="6" />
                                <cc1:FilteredTextBoxExtender ValidChars="1234567890." FilterType="Custom" ID="FTE_Txt_Enero"
                                    runat="server" TargetControlID="Txt_Enero">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                            <td style="text-align: right; width: 16%;">
                                <asp:Label ID="Lbl_Febrero" runat="server" Text="* Febrero">
                                </asp:Label>
                            </td>
                            <td style="text-align: right; width: 16%;">
                                <asp:TextBox ID="Txt_Febrero" runat="server" Width="95%" MaxLength="6" />
                                <cc1:FilteredTextBoxExtender ValidChars="1234567890." FilterType="Custom" ID="FTE_Txt_Febrero"
                                    runat="server" TargetControlID="Txt_Febrero">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                            <td style="text-align: right; width: 16%;">
                                <asp:Label ID="Lbl_Marzo" runat="server" Text="* Marzo">
                                </asp:Label>
                            </td>
                            <td style="text-align: right; width: 16%;">
                                <asp:TextBox ID="Txt_Marzo" runat="server" Width="95%" MaxLength="6" />
                                <cc1:FilteredTextBoxExtender ValidChars="1234567890." FilterType="Custom" ID="FTE_Txt_Marzo"
                                    runat="server" TargetControlID="Txt_Marzo">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; width: 16%;">
                                <asp:Label ID="Lbl_Abril" runat="server" Text="* Abril">
                                </asp:Label>
                            </td>
                            <td style="text-align: right; width: 16%;">
                                <asp:TextBox ID="Txt_Abril" runat="server" Width="95%" MaxLength="6" />
                                <cc1:FilteredTextBoxExtender ValidChars="1234567890." FilterType="Custom" ID="FTE_Txt_Abril"
                                    runat="server" TargetControlID="Txt_Abril">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                            <td style="text-align: right; width: 16%;">
                                <asp:Label ID="Lbl_Mayo" runat="server" Text="* Mayo">
                                </asp:Label>
                            </td>
                            <td style="text-align: right; width: 16%;">
                                <asp:TextBox ID="Txt_Mayo" runat="server" Width="95%" MaxLength="6" />
                                <cc1:FilteredTextBoxExtender ValidChars="1234567890." FilterType="Custom" ID="FTE_Txt_Mayo"
                                    runat="server" TargetControlID="Txt_Mayo">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                            <td style="text-align: right; width: 16%;">
                                <asp:Label ID="Lbl_Junio" runat="server" Text="* Junio">
                                </asp:Label>
                            </td>
                            <td style="text-align: right; width: 16%;">
                                <asp:TextBox ID="Txt_Junio" runat="server" Width="95%" MaxLength="6" />
                                <cc1:FilteredTextBoxExtender ValidChars="1234567890." FilterType="Custom" ID="FTE_Txt_Junio"
                                    runat="server" TargetControlID="Txt_Junio">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; width: 16%;">
                                <asp:Label ID="Lbl_Julio" runat="server" Text="* Julio">
                                </asp:Label>
                            </td>
                            <td style="text-align: right; width: 16%;">
                                <asp:TextBox ID="Txt_Julio" runat="server" Width="95%" MaxLength="6" />
                                <cc1:FilteredTextBoxExtender ValidChars="1234567890." FilterType="Custom" ID="FTE_Txt_Julio"
                                    runat="server" TargetControlID="Txt_Julio">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                            <td style="text-align: right; width: 16%;">
                                <asp:Label ID="Lbl_Agosto" runat="server" Text="* Agosto">
                                </asp:Label>
                            </td>
                            <td style="text-align: right; width: 16%;">
                                <asp:TextBox ID="Txt_Agosto" runat="server" Width="95%" MaxLength="6" />
                                <cc1:FilteredTextBoxExtender ValidChars="1234567890." FilterType="Custom" ID="FTE_Txt_Agosto"
                                    runat="server" TargetControlID="Txt_Agosto">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                            <td style="text-align: right; width: 16%;">
                                <asp:Label ID="Lbl_Septiembre" runat="server" Text="* Spetiembre">
                                </asp:Label>
                            </td>
                            <td style="text-align: right; width: 16%;">
                                <asp:TextBox ID="Txt_Septiembre" runat="server" Width="95%" MaxLength="6" />
                                <cc1:FilteredTextBoxExtender ValidChars="1234567890." FilterType="Custom" ID="FTE_Txt_Septiembre"
                                    runat="server" TargetControlID="Txt_Septiembre">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; width: 10%;">
                                <asp:Label ID="Lbl_Octubre" runat="server" Text="* Octubre">
                                </asp:Label>
                            </td>
                            <td style="text-align: right; width: 16%;">
                                <asp:TextBox ID="Txt_Octubre" runat="server" Width="95%" MaxLength="6" />
                                <cc1:FilteredTextBoxExtender ValidChars="1234567890." FilterType="Custom" ID="FTE_Txt_Octubre"
                                    runat="server" TargetControlID="Txt_Octubre">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                            <td style="text-align: right; width: 16%;">
                                <asp:Label ID="Lbl_Noviembre" runat="server" Text="* Noviembre">
                                </asp:Label>
                            </td>
                            <td style="text-align: right; width: 16%;">
                                <asp:TextBox ID="Txt_Noviembre" runat="server" Width="95%" MaxLength="6" />
                                <cc1:FilteredTextBoxExtender ValidChars="1234567890." FilterType="Custom" ID="FTE_Txt_Noviembre"
                                    runat="server" TargetControlID="Txt_Noviembre">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                            <td style="text-align: right; width: 16%;">
                                <asp:Label ID="Lbl_Diciembre" runat="server" Text="* Diciembre">
                                </asp:Label>
                            </td>
                            <td style="text-align: right; width: 16%;">
                                <asp:TextBox ID="Txt_Diciembre" runat="server" Width="95%" MaxLength="6" />
                                <cc1:FilteredTextBoxExtender ValidChars="1234567890." FilterType="Custom" ID="FTE_Txt_Diciembre"
                                    runat="server" TargetControlID="Txt_Diciembre">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                    </table>
                    <table width="98%" class="estilo_fuente">
                        <tr>
                            <td align="center" colspan="4">
                                <tr align="center">
                                    <td colspan="4">
                                        <asp:GridView ID="Grid_Recargos" runat="server" AllowPaging="True" AllowSorting="True"
                                            EmptyDataText="&quot;No se encontraron registros&quot;" AutoGenerateColumns="False"
                                            CssClass="GridView_1" HeaderStyle-CssClass="tblHead" Style="white-space: normal;"
                                            Width="100%" PageSize="100"
                                            OnRowCommand = "Borrar_Recargo" >
                                            <Columns>
                                                <asp:BoundField DataField="RECARGO_ID" HeaderText="Recargo ID" />
                                                <asp:TemplateField HeaderText="Año" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" >
                                                    <itemtemplate>
                                                        <asp:TextBox ID="Txt_Grid_Anio" runat="server"  
                                                            style="width:96%; text-align:right;" MaxLength="4"
                                                            Visible='<%# Grid_Recargos_Editable %>'
                                                            Text='<%# Bind("ANIO") %>' >
                                                        </asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="Fte_Txt_Grid_Anio" runat="server" 
                                                            FilterType=" Numbers" TargetControlID="Txt_Grid_Anio" />
                                                        <asp:Label ID="Lbl_Txt_Grid_Anio" runat="server" 
                                                            style="width:96%; text-align:right;" 
                                                            Visible='<%# !(bool)Grid_Recargos_Editable %>'
                                                            Text='<%# Bind("ANIO") %>' >
                                                        </asp:Label>
                                                    </itemtemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Bimestre" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" >
                                                    <itemtemplate>
                                                        <asp:TextBox ID="Txt_Grid_Bimestre" runat="server" 
                                                            style="width:96%; text-align:right;"  MaxLength="1"
                                                            Visible='<%# Grid_Recargos_Editable %>'
                                                            Text='<%# Bind("BIMESTRE") %>' >
                                                        </asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="Fte_Txt_Grid_Bimestre" runat="server" 
                                                            FilterType=" Numbers" TargetControlID="Txt_Grid_Bimestre" />
                                                        <asp:Label ID="Lbl_Txt_Grid_Bimestre" runat="server" 
                                                            style="width:96%; text-align:right;" 
                                                            Visible='<%# !(bool)Grid_Recargos_Editable %>'
                                                            Text='<%# Bind("BIMESTRE") %>' >
                                                        </asp:Label>
                                                    </itemtemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Ene" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="5%" >
                                                    <itemtemplate>
                                                        <asp:TextBox ID="Txt_Grid_Enero" runat="server"  
                                                            style="width:96%; text-align:right;"  MaxLength="7"
                                                            Visible='<%# Grid_Recargos_Editable %>'
                                                            Text='<%# Bind("ENERO", "{0:0.##}") %>' >
                                                        </asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="Fte_Txt_Grid_Enero" runat="server" 
                                                            FilterType="Numbers,Custom" TargetControlID="Txt_Grid_Enero" 
                                                            ValidChars="." />
                                                        <asp:Label ID="Lbl_Txt_Grid_Enero" runat="server" 
                                                            style="width:90%; text-align:right; padding-right:3px;" 
                                                            Visible='<%# !(bool)Grid_Recargos_Editable %>'
                                                            Text='<%# Bind("ENERO", "{0:0.##}") %>' >
                                                        </asp:Label>
                                                    </itemtemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Feb" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="5%" >
                                                    <itemtemplate>
                                                        <asp:TextBox ID="Txt_Grid_Febrero" runat="server" 
                                                            style="width:96%; text-align:right;"  MaxLength="7"
                                                            Visible='<%# Grid_Recargos_Editable %>'
                                                            Text='<%# Bind("FEBRERO", "{0:0.##}") %>' >
                                                        </asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="Fte_Txt_Grid_Febrero" runat="server" 
                                                            FilterType="Numbers,Custom" TargetControlID="Txt_Grid_Febrero" 
                                                            ValidChars="." />
                                                        <asp:Label ID="Lbl_Txt_Grid_Febrero" runat="server" 
                                                            style="width:90%; text-align:right; padding-right:3px;" 
                                                            Visible='<%# !(bool)Grid_Recargos_Editable %>'
                                                            Text='<%# Bind("FEBRERO", "{0:0.##}") %>' >
                                                        </asp:Label>
                                                    </itemtemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Mar" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="5%" >
                                                    <itemtemplate>
                                                        <asp:TextBox ID="Txt_Grid_Marzo" runat="server" 
                                                            style="width:96%; text-align:right;"  MaxLength="7"
                                                            Visible='<%# Grid_Recargos_Editable %>'
                                                            Text='<%# Bind("MARZO", "{0:0.##}") %>' >
                                                        </asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="Fte_Txt_Grid_Marzo" runat="server" 
                                                            FilterType="Numbers,Custom" TargetControlID="Txt_Grid_Marzo" 
                                                            ValidChars="." />
                                                        <asp:Label ID="Lbl_Txt_Grid_Marzo" runat="server" 
                                                            style="width:90%; text-align:right; padding-right:3px;" 
                                                            Visible='<%# !(bool)Grid_Recargos_Editable %>'
                                                            Text='<%# Bind("MARZO", "{0:0.##}") %>' >
                                                        </asp:Label>
                                                    </itemtemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Abr" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="5%" >
                                                    <itemtemplate>
                                                        <asp:TextBox ID="Txt_Grid_Abril" runat="server" 
                                                            style="width:96%; text-align:right;"  MaxLength="7"
                                                            Visible='<%# Grid_Recargos_Editable %>'
                                                            Text='<%# Bind("ABRIL", "{0:0.##}") %>' >
                                                        </asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="Fte_Txt_Grid_Abril" runat="server" 
                                                            FilterType="Numbers,Custom" TargetControlID="Txt_Grid_Abril" 
                                                            ValidChars="." />
                                                        <asp:Label ID="Lbl_Txt_Grid_Abril" runat="server" 
                                                            style="width:90%; text-align:right; padding-right:3px;" 
                                                            Visible='<%# !(bool)Grid_Recargos_Editable %>'
                                                            Text='<%# Bind("ABRIL", "{0:0.##}") %>' >
                                                        </asp:Label>
                                                    </itemtemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="May" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="5%" >
                                                    <itemtemplate>
                                                        <asp:TextBox ID="Txt_Grid_Mayo" runat="server" 
                                                            style="width:96%; text-align:right;"  MaxLength="7"
                                                            Visible='<%# Grid_Recargos_Editable %>'
                                                            Text='<%# Bind("MAYO", "{0:0.##}") %>' >
                                                        </asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="Fte_Txt_Grid_Mayo" runat="server" 
                                                            FilterType="Numbers,Custom" TargetControlID="Txt_Grid_Mayo" 
                                                            ValidChars="." />
                                                        <asp:Label ID="Lbl_Txt_Grid_Mayo" runat="server" 
                                                            style="width:90%; text-align:right; padding-right:3px;" 
                                                            Visible='<%# !(bool)Grid_Recargos_Editable %>'
                                                            Text='<%# Bind("MAYO", "{0:0.##}") %>' >
                                                        </asp:Label>
                                                    </itemtemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Jun" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="5%" >
                                                    <itemtemplate>
                                                        <asp:TextBox ID="Txt_Grid_Junio" runat="server" 
                                                            style="width:96%; text-align:right;"  MaxLength="7"
                                                            Visible='<%# Grid_Recargos_Editable %>'
                                                            Text='<%# Bind("JUNIO", "{0:0.##}") %>' >
                                                        </asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="Fte_Txt_Grid_Junio" runat="server" 
                                                            FilterType="Numbers,Custom" TargetControlID="Txt_Grid_Junio" 
                                                            ValidChars="." />
                                                        <asp:Label ID="Lbl_Txt_Grid_Junio" runat="server" 
                                                            style="width:90%; text-align:right; padding-right:3px;" 
                                                            Visible='<%# !(bool)Grid_Recargos_Editable %>'
                                                            Text='<%# Bind("JUNIO", "{0:0.##}") %>' >
                                                        </asp:Label>
                                                    </itemtemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Jul" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="5%" >
                                                    <itemtemplate>
                                                        <asp:TextBox ID="Txt_Grid_Julio" runat="server" 
                                                            style="width:96%; text-align:right;"  MaxLength="7"
                                                            Visible='<%# Grid_Recargos_Editable %>'
                                                            Text='<%# Bind("JULIO", "{0:0.##}") %>' >
                                                        </asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="Fte_Txt_Grid_Julio" runat="server" 
                                                            FilterType="Numbers,Custom" TargetControlID="Txt_Grid_Julio" 
                                                            ValidChars="." />
                                                        <asp:Label ID="Lbl_Txt_Grid_Julio" runat="server" 
                                                            style="width:90%; text-align:right; padding-right:3px;" 
                                                            Visible='<%# !(bool)Grid_Recargos_Editable %>'
                                                            Text='<%# Bind("JULIO", "{0:0.##}") %>' >
                                                        </asp:Label>
                                                    </itemtemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Ago" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="5%" >
                                                    <itemtemplate>
                                                        <asp:TextBox ID="Txt_Grid_Agosto" runat="server" 
                                                            style="width:96%; text-align:right;"  MaxLength="7"
                                                            Visible='<%# Grid_Recargos_Editable %>'
                                                            Text='<%# Bind("AGOSTO", "{0:0.##}") %>' >
                                                        </asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="Fte_Txt_Grid_Agosto" runat="server" 
                                                            FilterType="Numbers,Custom" TargetControlID="Txt_Grid_Agosto" 
                                                            ValidChars="." />
                                                        <asp:Label ID="Lbl_Txt_Grid_Agosto" runat="server" 
                                                            style="width:90%; text-align:right; padding-right:3px;" 
                                                            Visible='<%# !(bool)Grid_Recargos_Editable %>'
                                                            Text='<%# Bind("AGOSTO", "{0:0.##}") %>' >
                                                        </asp:Label>
                                                    </itemtemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sep" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="5%" >
                                                    <itemtemplate>
                                                        <asp:TextBox ID="Txt_Grid_Septiembre" runat="server" 
                                                            style="width:96%; text-align:right;"  MaxLength="7"
                                                            Visible='<%# Grid_Recargos_Editable %>'
                                                            Text='<%# Bind("SEPTIEMBRE", "{0:0.##}") %>' >
                                                        </asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="Fte_Txt_Grid_Septiembre" runat="server" 
                                                            FilterType="Numbers,Custom" TargetControlID="Txt_Grid_Septiembre" 
                                                            ValidChars="." />
                                                        <asp:Label ID="Lbl_Txt_Grid_Septiembre" runat="server" 
                                                            style="width:90%; text-align:right; padding-right:3px;" 
                                                            Visible='<%# !(bool)Grid_Recargos_Editable %>'
                                                            Text='<%# Bind("SEPTIEMBRE", "{0:0.##}") %>' >
                                                        </asp:Label>
                                                    </itemtemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Oct" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="5%" >
                                                    <itemtemplate>
                                                        <asp:TextBox ID="Txt_Grid_Octubre" runat="server" 
                                                            style="width:96%; text-align:right;"  MaxLength="7"
                                                            Visible='<%# Grid_Recargos_Editable %>'
                                                            Text='<%# Bind("OCTUBRE", "{0:0.##}") %>' >
                                                        </asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="Fte_Txt_Grid_Octubre" runat="server" 
                                                            FilterType="Numbers,Custom" TargetControlID="Txt_Grid_Octubre" 
                                                            ValidChars="." />
                                                        <asp:Label ID="Lbl_Txt_Grid_Octubre" runat="server" 
                                                            style="width:90%; text-align:right; padding-right:3px;" 
                                                            Visible='<%# !(bool)Grid_Recargos_Editable %>'
                                                            Text='<%# Bind("OCTUBRE", "{0:0.##}") %>' >
                                                        </asp:Label>
                                                    </itemtemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Nov" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="5%" >
                                                    <itemtemplate>
                                                        <asp:TextBox ID="Txt_Grid_Noviembre" runat="server" 
                                                            style="width:96%; text-align:right;"  MaxLength="7"
                                                            Visible='<%# Grid_Recargos_Editable %>'
                                                            Text='<%# Bind("NOVIEMBRE", "{0:0.##}") %>' >
                                                        </asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="Fte_Txt_Grid_Noviembre" runat="server" 
                                                            FilterType="Numbers,Custom" TargetControlID="Txt_Grid_Noviembre" 
                                                            ValidChars="." />
                                                        <asp:Label ID="Lbl_Txt_Grid_Noviembre" runat="server" 
                                                            style="width:90%; text-align:right; padding-right:3px;" 
                                                            Visible='<%# !(bool)Grid_Recargos_Editable %>'
                                                            Text='<%# Bind("NOVIEMBRE", "{0:0.##}") %>' >
                                                        </asp:Label>
                                                    </itemtemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Dic" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="5%" >
                                                    <itemtemplate>
                                                        <asp:TextBox ID="Txt_Grid_Diciembre" runat="server" 
                                                            style="width:96%; text-align:right;"  MaxLength="7"
                                                            Visible='<%# Grid_Recargos_Editable %>'
                                                            Text='<%# Bind("DICIEMBRE", "{0:0.##}") %>' >
                                                        </asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="Fte_Txt_Grid_Diciembre" runat="server" 
                                                            FilterType="Numbers,Custom" TargetControlID="Txt_Grid_Diciembre" 
                                                            ValidChars="." />
                                                        <asp:Label ID="Lbl_Txt_Grid_Diciembre" runat="server" 
                                                            style="width:90%; text-align:right; padding-right:3px;" 
                                                            Visible='<%# !(bool)Grid_Recargos_Editable %>'
                                                            Text='<%# Bind("DICIEMBRE", "{0:0.##}") %>' >
                                                        </asp:Label>
                                                    </itemtemplate>
                                                </asp:TemplateField>
                                                
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="4%" >
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="Btn_Eliminar_Seleccionado" runat="server" Height="20px" 
                                                        ImageUrl="~/paginas/imagenes/paginas/delete.png"
                                                        OnClientClick = "return confirm('¿Está seguro que desea eliminar el registro?')"
                                                        ToolTip="Borrar_Recepcion" Width="20px"  CommandName="Erase" 
                                                        CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <SelectedRowStyle CssClass="GridSelected" />
                                            <PagerStyle CssClass="GridHeader" />
                                            <HeaderStyle CssClass="tblHead" />
                                            <AlternatingRowStyle CssClass="GridAltItem" />
                                        </asp:GridView>
                                        <br />
                                    </td>
                                </tr>
                            </td>
                            <tr>
                                <td align="left" colspan="2" />
                            </tr>
                            <td colspan="2" align="left" />
                        </tr>
                    </table>
                </center>
            </div>
            <br />
            <br />
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

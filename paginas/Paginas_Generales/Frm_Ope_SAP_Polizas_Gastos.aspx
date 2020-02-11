<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_SAP_Polizas_Gastos.aspx.cs" Inherits="paginas_Paginas_Generales_Frm_Ope_SAP_Polizas_Gastos" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" 
        EnableScriptGlobalization="True" />
    <div style="width: 95%;">
        <asp:UpdatePanel ID="Upl_Contenedor" runat="server">
            <ContentTemplate>
                <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upl_Contenedor"
                    DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                        </div>
                        <div class="processMessage" id="div_progress">
                            <img alt="" src="../Imagenes/paginas/Updating.gif" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <table width="98%" border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="4" class="label_titulo">Generaci&oacute;n de p&oacute;lizas de gastos</td>
                    </tr>
                    <tr align="left">
                        <td colspan="4" >
                            <asp:Image ID="Img_Error" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" /> &nbsp; 
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" 
                                ForeColor="#990000"></asp:Label>
                        </td>
                    </tr>
                    <tr class="barra_busqueda" align="right">
                        <td colspan="4" align="left" valign="middle">
                           <table width = "100%">
                            <tr>
                                <td>
                                    <asp:ImageButton ID="Btn_Generar_Archivo_Polizas" runat="server" 
                                        ImageUrl="~/paginas/imagenes/gridview/grid_docto.png"
                                        CssClass="Img_Button" ToolTip="Generar archivo de p&oacute;lizas" 
                                        TabIndex="1" onclick="Btn_Generar_Archivo_Polizas_Click" />
                                    <asp:ImageButton ID="Btn_Salir" runat="server" 
                                        CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                            ToolTip="Inicio" TabIndex="2" onclick="Btn_Salir_Click" />
                                </td>
                                
                                <td align = "right">
                                    Búsqueda:
                                    <asp:TextBox ID="Txt_Busqueda" runat="server" Width="180" MaxLength="100"  TabIndex="3"></asp:TextBox>
                                    <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                                        WatermarkText="<Ingrese Folio>" TargetControlID="Txt_Busqueda" />
                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" runat="server" TargetControlID="Txt_Busqueda" 
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ- "/>
                                    <asp:ImageButton ID="Btn_Buscar" runat="server" 
                                        ImageUrl="~/paginas/imagenes/paginas/busqueda.png" TabIndex="4" onclick="Btn_Buscar_Click" 
                                         />
                                </td>
                            </tr>
                           </table>
                        </td>
                    </tr>
                    
                    <tr>
                        <td colspan="2">
                            Fecha Inicial:&nbsp;&nbsp;
                            <asp:TextBox ID="Txt_Fecha_Inicio" runat="server" Width="90px" ReadOnly="false"
                                ontextchanged="Txt_Fecha_TextChanged"  TabIndex="5" ></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="Txt_Fecha_Inicio_FilteredTextBoxExtender" 
                                runat="server" TargetControlID="Txt_Fecha_Inicio" 
                                FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" ValidChars="/_"/>
                            <cc1:CalendarExtender ID="Txt_Fecha_Inicio_CalendarExtender" runat="server" 
                                TargetControlID="Txt_Fecha_Inicio" PopupButtonID="Btn_Fecha_Inicio" Format="dd/MMM/yyyy"
                                OnClientDateSelectionChanged="F_Inicial" >
                            </cc1:CalendarExtender>
                            <asp:ImageButton ID="Btn_Fecha_Inicio" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                ToolTip="Seleccione la Fecha Inicial"/>
                        </td>
                        <td colspan="2">
                            Fecha Final:&nbsp;&nbsp;
                            <asp:TextBox ID="Txt_Fecha_Final" runat="server" Width="90px" ReadOnly="false"
                                ontextchanged="Txt_Fecha_TextChanged"  TabIndex="6" ></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Fecha_Final" 
                                runat="server" TargetControlID="Txt_Fecha_Final" 
                                FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" ValidChars="/_"/>
                            <cc1:CalendarExtender ID="Cex_Txt_Fecha_Final" runat="server" 
                                TargetControlID="Txt_Fecha_Final" PopupButtonID="Btn_Fecha_Final" Format="dd/MMM/yyyy"
                                OnClientDateSelectionChanged="F_Final" >
                            </cc1:CalendarExtender>
                            <asp:ImageButton ID="Btn_Fecha_Final" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                ToolTip="Seleccione la Fecha Final" />
                        </td>  
                    </tr>
                    
                    <tr><td colspan="4"><hr />Gastos <br /></td></tr>
                    
                    <tr align="center">
                        <td colspan="4">
                            
                            <asp:GridView ID="Grid_Gastos" runat="server" AutoGenerateColumns="False" 
                                CssClass="GridView_1" style="white-space:normal" 
                                GridLines="None" Width = "100%" 
                                DataKeyNames="GASTO_ID,FOLIO">
                                <RowStyle CssClass="GridItem" />
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="Chk_Seleccionar_Todos_Gastos" runat="server" 
                                                oncheckedchanged="Chk_Seleccionar_Todos_Gastos_CheckedChanged" AutoPostBack="true" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="Chk_Gasto_Seleccionado" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="GASTO_ID" HeaderText="No. Gasto" Visible="false" />
                                    <asp:BoundField DataField="FOLIO" HeaderText="Folio" Visible="true">
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CLAVE_NOMBRE_DEPENDENCIA" HeaderText="Dependencia" Visible="true">
                                        <HeaderStyle HorizontalAlign="Left" Width="60%" />
                                        <ItemStyle HorizontalAlign="Left" Width="60%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="COSTO_TOTAL_GASTO" HeaderText="Total" Visible="true">
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" Visible="false" />
                                </Columns>
                                <PagerStyle CssClass="GridHeader" />
                                <SelectedRowStyle CssClass="GridSelected" />
                                <HeaderStyle CssClass="GridHeader" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

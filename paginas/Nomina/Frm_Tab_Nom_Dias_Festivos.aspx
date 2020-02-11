<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Tab_Nom_Dias_Festivos.aspx.cs" Inherits="paginas_Nomina_Frm_Tab_Nom_Dias_Festivos" Title="Tabla de Días Festivos" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">    

    <asp:ScriptManager ID="ScriptManager_Dias_Festivos" runat="server" EnableScriptGlobalization = "true" EnableScriptLocalization = "True"/>
        <asp:UpdatePanel ID="Upd_Panel" runat="server">        
        <ContentTemplate>
        
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div class="processMessage" id="div_progress"><img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>                
            </asp:UpdateProgress>
            
            <div id="Div_Dias_Festivos" style="background-color:#ffffff; width:100%; height:100%;">
                <table width="98%" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="4" class="label_titulo">Tabla de Días Festivos</td>
                    </tr>
                    <tr>
                        <td colspan="4">&nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
                    <tr class="barra_busqueda" align="right">
                        <td colspan="2" align = "left">
                           <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button" TabIndex="1"
                                ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" onclick="Btn_Nuevo_Click" />
                            <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button" TabIndex="2"
                                ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" onclick="Btn_Modificar_Click" />
                            <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar" CssClass="Img_Button" TabIndex="3"
                                ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png" onclick="Btn_Eliminar_Click"
                                OnClientClick="return confirm('¿Está seguro de eliminar el Área seleccionada?');"/>
                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" TabIndex="4"
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" onclick="Btn_Salir_Click"/>
                        </td>
                        <td colspan="2">Busqueda
                            <asp:TextBox ID="Txt_Busqueda_Dias_Festivos" runat="server" MaxLength="100" TabIndex="5" Width="150px" ToolTip="Buscar por ID"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Dias_Festivos" runat="server" WatermarkCssClass="watermarked"
                                WatermarkText="<Ingrese el ID>" TargetControlID="Txt_Busqueda_Dias_Festivos" />
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Dias_Festivos" runat="server" 
                                TargetControlID="Txt_Busqueda_Dias_Festivos" FilterType="Numbers">
                            </cc1:FilteredTextBoxExtender>
                            <asp:ImageButton ID="Btn_Busqueda_Dias_Festivos" runat="server" ToolTip="Consultar" TabIndex="6" 
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" onclick="Btn_Busqueda_Dias_Festivos_Click" />
                        </td>                                              
                    </tr>
                    <tr>
                        <td style="width:20%;text-align:left;">
                            Día ID
                        </td>
                        <td style="width:30%;text-align:left;">
                            <asp:TextBox ID="Txt_Dia_ID" runat="server" ReadOnly="True" Width="98%" />
                        </td>
                        <td style="width:20%;text-align:left;">
                            
                        </td>
                        <td style="width:30%;text-align:left;">
                          
                        </td>                          
                    </tr>
                    <tr>
                        <td style="width:20%;text-align:left;">
                            *Año
                        </td>
                        <td style="width:30%;text-align:left;">
                            <asp:DropDownList ID="Cmb_Calendario_Nomina" runat="server" Width="100%"/>                            
                        </td>
                        <td style="width:20%;text-align:left;">
                            &nbsp;&nbsp;*Fecha
                        </td>
                        <td style="width:30%;text-align:left;">
                            <asp:TextBox ID="Txt_Fecha_Dia_Festivo" runat="server" Width="83%" TabIndex="7" MaxLength="11"/>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Fecha_Dia_Festivo" runat="server" TargetControlID="Txt_Fecha_Dia_Festivo" 
                                FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars="/"/>                                              
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Fecha_Dia_Festivo" runat="server" 
                                TargetControlID="Txt_Fecha_Dia_Festivo" WatermarkCssClass="watermarked" WatermarkText="Dia/Mes/Año" />
                            <cc1:CalendarExtender ID="CE_Txt_Fecha_Dia_Festivo" runat="server"  PopupButtonID="Btn_Fecha"
                                TargetControlID="Txt_Fecha_Dia_Festivo" Format="dd/MMM/yyyy"/>
                            <asp:ImageButton ID="Btn_Fecha" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                ToolTip="Seleccione la Fecha"/>                                                        
                        </td>                          
                    </tr>                    
                    <tr>                    
                        <td style="width:20%;text-align:left;">
                            *Conmemora
                        </td>
                        <td colspan="3" style="width:80%;text-align:left;">
                            <asp:TextBox ID="Txt_Descripcion_Dia_Festivo" runat="server" Width="99.5%" TabIndex="8" MaxLength="100"/> 
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Descripcion_Dia_Festivo" runat="server" TargetControlID="Txt_Descripcion_Dia_Festivo" 
                                FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ "/>                       
                        </td>
                    </tr>                    
                    <tr>
                        <td style="width:20%;text-align:left;vertical-align:top">
                            Comentarios
                        </td>
                        <td colspan="3" style="width:80%;text-align:left;">
                            <asp:TextBox ID="Txt_Comentarios_Dia_Festivo" runat="server" TabIndex="9" MaxLength="250"
                                TextMode="MultiLine" Width="99.5%" AutoPostBack="True" Height="40px"/>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Comentarios_Dia_Festivo" runat="server" WatermarkCssClass="watermarked"
                                TargetControlID ="Txt_Comentarios_Dia_Festivo" WatermarkText="Límite de Caractes 250"/>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Comentarios_Dia_Festivo" runat="server" TargetControlID="Txt_Comentarios_Dia_Festivo" 
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ "/>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">&nbsp;</td>
                    </tr>
                    <tr align="center">
                        <td colspan="4">
                            <asp:GridView ID="Grid_Dias_Festivos" runat="server" AllowPaging="True" 
                                AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None"
                                PageSize="5" onpageindexchanging="Grid_Dias_Festivos_PageIndexChanging" 
                                onselectedindexchanged="Grid_Dias_Festivos_SelectedIndexChanged">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="7%" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="Dia_ID" HeaderText="Día ID" 
                                        Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" Visible="True" 
                                        DataFormatString="{0:dd/MMM/yyyy}">
                                        <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                        <ItemStyle HorizontalAlign="Left" Width="25%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Descripcion" HeaderText="Conmemora" 
                                        Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" Width="50%" />
                                        <ItemStyle HorizontalAlign="left" Width="50%" />
                                    </asp:BoundField>                                    
                                    <asp:BoundField DataField="Comentarios" HeaderText="Comentarios" Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NOMINA_ID" HeaderText="">
                                        <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                                    </asp:BoundField>                                    
                                </Columns>
                                <SelectedRowStyle CssClass="GridSelected" />
                                <PagerStyle CssClass="GridHeader" />
                                <HeaderStyle CssClass="GridHeader" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>                        
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Pre_Fechas_Aplicacion.aspx.cs" Inherits="paginas_Predial_Frm_Ope_Pre_Fechas_Aplicacion" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
    <script type="text/javascript" language="javascript">
        function pageLoad() 
        {
            $('[id*=Txt_Motiv').keyup(function() {var Caracteres =  $(this).val().length;if (Caracteres > 250) {this.value = this.value.substring(0, 250);$(this).css("background-color", "Yellow");$(this).css("color", "Red");}else{$(this).css("background-color", "White");$(this).css("color", "Black");}$('#Contador_Caracteres_Motivo_Dia_Inhabil').text('Carácteres Ingresados [ ' + Caracteres + ' ]/[ 250 ]');});
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
        
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            
            <div id="Div_Contenido" style="background-color:#ffffff; width:100%; height:100%;">
                    
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="4" class="label_titulo">
                            Configuración de fecha de aplicación
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">&nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
               </table>             
            
               <table width="98%"  border="0" cellspacing="0">
                <tr align="center">
                    <td colspan="2">                
                        <div style="width:99%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold; font-style: normal; font-variant: normal; font-family: fantasy; height:32px"  >
                            <table style="width:100%;height:28px;">
                                <tr>
                                    <td align="left" style="width:59%;">
                                        <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" 
                                            CssClass="Img_Button" TabIndex="1"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" 
                                            onclick="Btn_Nuevo_Click" />
                                        <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" 
                                            CssClass="Img_Button" TabIndex="2"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" 
                                            onclick="Btn_Modificar_Click" />
                                        <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" 
                                            CssClass="Img_Button" TabIndex="5"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                            onclick="Btn_Salir_Click" />
                                    </td>
                                    <td align="right" style="width:41%;">
                                        <table style="width:100%;height:28px;">
                                            <tr>
                                                <td style="vertical-align:middle;text-align:right;width:20%;">B&uacute;squeda:</td>
                                                <td style="width:55%;">
                                                    <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100" TabIndex="5"  ToolTip = "Buscar Fecha Aplicacion" Width="180px" Enabled="false"/>
                                                    <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                                                        WatermarkText="<Buscar Fecha Aplicacion>" TargetControlID="Txt_Busqueda" />
                                                    <cc1:CalendarExtender ID="DTP_Fecha_Busqueda" runat="server" 
                                                        TargetControlID="Txt_Busqueda" Format="dd/MMM/yyyy" Enabled="True" PopupButtonID="Btn_Fecha_Busqueda"/>
                                                     <asp:ImageButton ID="Btn_Fecha_Busqueda" runat="server"
                                                        ImageUrl="../imagenes/paginas/SmallCalendar.gif" style="vertical-align:top;"
                                                        Height="18px" CausesValidation="false" Enabled="false"/>
                                                </td>
                                                <td style="vertical-align:middle;width:5%;" >
                                                    <asp:ImageButton ID="Btn_Buscar" runat="server" TabIndex="6"
                                                        ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Buscar" 
                                                        onclick="Btn_Buscar_Fechas_Click" />
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
                    <%------------------ Fechas de aplicación ------------------%>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            Fecha de alta
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Fecha_Alta" runat="server" Width="96.4%" />
                        </td>
                        <td style="text-align:left;width:20%;">
                            *Estatus
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="99%" TabIndex="7">
                                <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE" />
                                <asp:ListItem Text="VIGENTE" Value="VIGENTE" />
                                <asp:ListItem Text="BAJA" Value="BAJA" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    
                    <tr>
                        <td style="text-align:left;width:20%;">
                            *Fecha movimiento
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Fecha_Movimiento" runat="server" Width="84%" TabIndex="12" MaxLength="11" Height="18px" Enabled="false"/>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" 
                                TargetControlID="Txt_Fecha_Movimiento" WatermarkCssClass="watermarked" 
                                WatermarkText="Dia/Mes/Año" Enabled="True" />
                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" 
                                TargetControlID="Txt_Fecha_Movimiento" Format="dd/MMM/yyyy" Enabled="True" PopupButtonID="Img_Fecha_1"/>
                             <asp:ImageButton ID="Img_Fecha_1" runat="server"
                                ImageUrl="../imagenes/paginas/SmallCalendar.gif" style="vertical-align:top;"
                                Height="18px" CausesValidation="false" Enabled="false"/>
                        </td>
                        <td style="text-align:left;width:20%;">
                            *Fecha aplicación
                        </td>
                        <td style="width:31.5%;text-align:left;">
                                <asp:TextBox ID="Txt_Fecha_Aplicacion" runat="server" Width="84%" TabIndex="12" MaxLength="11" Height="18px" Enabled="false"/>
                                <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Fecha_Final" runat="server" 
                                    TargetControlID="Txt_Fecha_Aplicacion" WatermarkCssClass="watermarked" 
                                    WatermarkText="Dia/Mes/Año" Enabled="True" />
                                <cc1:CalendarExtender ID="CE_Txt_Fecha_Final" runat="server" 
                                    TargetControlID="Txt_Fecha_Aplicacion" Format="dd/MMM/yyyy" Enabled="True" PopupButtonID="Img_Fecha_2"/>
                                 <asp:ImageButton ID="Img_Fecha_2" runat="server"
                                    ImageUrl="../imagenes/paginas/SmallCalendar.gif" style="vertical-align:top;"
                                    Height="18px" CausesValidation="false" Enabled="false"/>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%; vertical-align:top;">
                            *Motivo
                        </td>
                        <td colspan="3" style="text-align:left;width:80%;">
                            <asp:TextBox ID="Txt_Motivo" runat="server" TabIndex="10" MaxLength="250"
                                TextMode="MultiLine" Width="98%"/>
                            <span id="Contador_Caracteres_Motivo_Dia_Inhabil" class="watermarked"></span>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Txt_Motivo" runat="server" WatermarkCssClass="watermarked"
                                TargetControlID ="Txt_Motivo" WatermarkText="Límite de Caractes 250"/>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Motivo" runat="server" 
                                TargetControlID="Txt_Motivo" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ "/>
                        </td>
                    </tr>
                    <tr><td>
                        <asp:TextBox ID="Txt_Id" runat="server" Width="96.4%" Visible="false"/>
                        </td></tr>
                    <tr style="background-color: #36C;">
                        <td style="text-align:left; font-size:15px; color:#FFF;" colspan="4" >
                            Fechas de aplicación
                        </td>
                    </tr>
                    <tr align="center">
                        <td colspan="4">
                            <asp:GridView ID="Grid_Fechas" runat="server" AllowPaging="True" CssClass="GridView_1"
                                AutoGenerateColumns="False" PageSize="5" Width="100%" GridLines="None"
                                AllowSorting="True" HeaderStyle-CssClass="tblHead" 
                                onpageindexchanging="Grid_Fechas_PageIndexChanging" 
                                onselectedindexchanged="Grid_Fechas_SelectedIndexChanged">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="5%" HorizontalAlign="Center"/>
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="FECHA_APLICACION_ID" HeaderText="ID" HeaderStyle-Width="15%" Visible="false" />
                                    <asp:BoundField DataField="FECHA_MOVIMIENTO" HeaderText="Fecha Movimiento" HeaderStyle-Width="15%" DataFormatString="{0:dd/MMM/yyyy}"/>
                                    <asp:BoundField DataField="FECHA_APLICACION" HeaderText="Fecha Aplicación" HeaderStyle-Width="15%" DataFormatString="{0:dd/MMM/yyyy}"/>
                                    <asp:BoundField DataField="MOTIVO" HeaderText="Motivo" />
                                    <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" HeaderStyle-Width="15%" />
                                </Columns>
                                <SelectedRowStyle CssClass="GridSelected" />
                                <PagerStyle CssClass="GridHeader" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
                
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


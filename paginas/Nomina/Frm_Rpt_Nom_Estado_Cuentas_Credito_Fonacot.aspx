<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Rpt_Nom_Estado_Cuentas_Credito_Fonacot.aspx.cs" Inherits="paginas_Nomina_Frm_Rpt_Nom_Estado_Cuentas_Credito_Fonacot" Title="Reporte Estado De Cuenta Creditos Fonacot"%>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager_Polizas" runat="server" />
                <div id="Div_Reporte_Balance_Mensual" style="background-color:#ffffff; width:100%; height:100%;">    
                    <table width="100%" class="estilo_fuente">
                        <tr align="center">
                            <td class="label_titulo">"Reporte Estado De Cuenta Creditos Fonacot</td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                                <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                                <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table width="98%"  border="0" cellspacing="0">
                        <tr align="center">
                            <td>                
                                <div align="right" class="barra_busqueda">                        
                                    <table style="width:100%;height:28px;">
                                        <tr>
                                            <td >
                                            <asp:UpdatePanel ID="Upd_Panel" runat="server">
                                                <ContentTemplate>
                                                <asp:UpdateProgress ID="Uprg_Polizas" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                                                    <ProgressTemplate>
                                                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                                                        <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                                                    </ProgressTemplate>
                                                </asp:UpdateProgress>
                                                <asp:ImageButton ID="Btn_Reporte" runat="server" ToolTip="Generar Reporte" 
                                                    CssClass="Img_Button" TabIndex="1"
                                                    ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png" 
                                                    onclick="Btn_Reporte_Click"/>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                               </td>
                                               <td align="left" style="width:50%;">
                                                    <asp:ImageButton ID="Btn_Imprimir_Excel" runat="server" 
                                                     ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png"
                                                      CssClass="Img_Button"  AlternateText="Imprimir Excel" 
                                                     ToolTip="Exportar Excel"
                                                     OnClick="Btn_Excel_Click" Visible="true"/> 
                                                <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" 
                                                    CssClass="Img_Button" TabIndex="2"
                                                    ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                                    onclick="Btn_Salir_Click"/>
                                            </td>
                                            <td align="right" style="width:50%;">&nbsp;</td>       
                                        </tr>         
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                    
                    <table width="98%" class="estilo_fuente"> 
                    <tr >
                           <td style=" width:20%">&nbsp;&nbsp;
                                        <asp:Label ID="Lbl_Empleado" runat="server" Text="*Empleado"></asp:Label>
                                    </td>
                                    <td style=" width:30%">
                                        <asp:TextBox ID="Txt_Nombre_Empleado" runat="server" MaxLength="100" TabIndex="11" Width="80%"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Nombre_Empleado" runat="server" TargetControlID="Txt_Nombre_Empleado"
                                            FilterType="Custom, UppercaseLetters, LowercaseLetters" ValidChars="ÑñáéíóúÁÉÍÓÚ. ">
                                        </cc1:FilteredTextBoxExtender>
                                        <asp:ImageButton ID="Btn_Buscar_Empleado" 
                                            runat="server" ToolTip="Consultar"
                                            TabIndex="12" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                            onclick="Btn_Buscar_Empleado_Click"/>
                                    </td>
                                    <td style=" width:50%"colspan="3">
                                        <asp:DropDownList ID="Cmb_Empleado" runat="server" TabIndex="13" Width="97%" />
                                    </td>
                        </tr>
                        <tr >
                            <td style=" width:20%">&nbsp;&nbsp;
                                        <asp:Label ID="Label1" runat="server" Text="Folio FONACOT"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txt_Folio_Fonacot" runat="server" MaxLength="10" TabIndex="11" Width="80%"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="Filt_Txt_Folio_Fonacot" runat="server" TargetControlID="Txt_Folio_Fonacot"
                                            FilterType="Numbers"/>
                                    </td>
                                    <td style=" width:20%">
                                        <asp:Label ID="Lbl_No_Credito" runat="server" Text="No. Credito"/>
                                    </td>                                    
                                    <td  style=" width:30%">
                                        <asp:TextBox ID="Txt_No_credito" runat="server" MaxLength="10" TabIndex="11" Width="93%"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="Filt_Txt_No_credito" runat="server" TargetControlID="Txt_No_credito"
                                            FilterType="Numbers"/>
                                    </td>
                                    <td>
                                    </td>
                        </tr>
                    </table>                       
               </div>
</asp:Content>

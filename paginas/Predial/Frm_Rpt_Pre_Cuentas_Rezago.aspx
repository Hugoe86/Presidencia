<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Rpt_Pre_Cuentas_Rezago.aspx.cs" Inherits="paginas_Predial_Frm_Rpt_Pre_Cuentas_Rezago"
    Title="Untitled Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">

    <script type="text/javascript" language="javascript">
        //Abrir una ventana modal
		function Abrir_Ventana_Modal(Url, Propiedades)
		{
			window.showModalDialog(Url, null, Propiedades);
		}
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <asp:ScriptManager ID="ScriptManager" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel"
                DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                    </div>
                    <div class="processMessage" id="div_progress">
                        <img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Contenido" style="background-color: #ffffff; width: 100%; height: 100%;">
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo">
                            Reporte de Cuentas con Rezago
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png"
                                Visible="false" />
                            <asp:Label ID="Lbl_Encabezado_Error" runat="server" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table width="98%" border="0" cellspacing="0">
                    <tr align="center">
                        <td>
                            <div style="width: 99%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold;
                                font-style: normal; font-variant: normal; font-family: fantasy; height: 32px">
                                <table style="width: 100%; height: 28px;">
                                    <tr>
                                        <td align="left" style="width: 35%;">
                                            <asp:ImageButton ID="Btn_Exportar_pdf" runat="server" AlternateText="Exportar a pdf"
                                                CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png"
                                                OnClick="Btn_Exportar_pdf_Click" TabIndex="1" ToolTip="Exportar a pdf" />
                                            <asp:ImageButton ID="Btn_Exportar_Excel" runat="server" AlternateText="Exportar a Excel"
                                                CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png"
                                                OnClick="Btn_Exportar_Excel_Click" TabIndex="1" ToolTip="Exportar a Excel" />
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                                TabIndex="5" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Salir_Click"
                                                AlternateText="Salir" />
                                        </td>
                                        <td align="right">
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
                <br />
                <table width="98%" class="estilo_fuente">
                    <tr style="background-color: #3366CC">
                        <td style="text-align: left; font-size: 15px; color: #FFFFFF;" colspan="4">
                            Filtros
                        </td>
                        <td style="text-align: right; font-size: 15px; color: #FFFFFF;">
                            <asp:ImageButton ID="Btn_Limpiar_Filtros" runat="server" Height="22px"
                                ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png" OnClick="Btn_Limpiar_Filtros_Click"
                                TabIndex="10" ToolTip="Limpiar Análisis de Rezago" Width="22px" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%">
                            Por Ubicación
                        </td>
                        <td style="text-align: left; width: 10%">
                            <asp:Label ID="Label1" runat="server" Text="Calle"></asp:Label>
                        </td>
                        <td style="text-align: left; width: 30%">
                            <asp:TextBox ID="Txt_Calle_Ubicacion" runat="server"></asp:TextBox>
                            <asp:HiddenField ID="Hdn_Calle_ID_Ubicacion" runat="server" />
                            <asp:ImageButton ID="Btn_Buscar_Ubicacion" runat="server" Height="22px" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png"
                                OnClick="Btn_Buscar_Ubicacion_Click" TabIndex="10" ToolTip="Seleccionar Calle y Colonia"
                                Width="22px" />
                        </td>
                        <td style="text-align: left; width: 10%">
                            <asp:Label ID="Label2" runat="server" Text="Colonia"></asp:Label>
                        </td>
                        <td style="text-align: left; width: 30%">
                            <asp:TextBox ID="Txt_Colonia_Ubicacion" runat="server"></asp:TextBox>
                            <asp:HiddenField ID="Hdn_Colonia_ID_Ubicacion" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%">
                            &nbsp;
                        </td>
                        <td style="text-align: left; width: 10%">
                            <asp:Label ID="Label3" runat="server" Text="Interior"></asp:Label>
                        </td>
                        <td style="text-align: left; width: 30%">
                            <asp:TextBox ID="Txt_Interior_Ubicacion" runat="server"></asp:TextBox>
                        </td>
                        <td style="text-align: left; width: 10%">
                            <asp:Label ID="Label4" runat="server" Text="Exterior"></asp:Label>
                        </td>
                        <td style="text-align: left; width: 30%">
                            <asp:TextBox ID="Txt_Exterior_Ubicacion" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%">
                            Por Notificación
                        </td>
                        <td style="text-align: left; width: 10%">
                            <asp:Label ID="Label5" runat="server" Text="Calle"></asp:Label>
                        </td>
                        <td style="text-align: left; width: 30%">
                            <asp:TextBox ID="Txt_Calle_Notificacion" runat="server"></asp:TextBox>
                            <asp:HiddenField ID="Hdn_Calle_ID_Notificacion" runat="server" />
                            <asp:ImageButton ID="Btn_Buscar_Notificacion" runat="server" Height="22px" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png"
                                OnClick="Btn_Buscar_Notificacion_Click" TabIndex="10" ToolTip="Seleccionar Calle y Colonia"
                                Width="22px" />
                        </td>
                        <td style="text-align: left; width: 10%">
                            <asp:Label ID="Label6" runat="server" Text="Colonia"></asp:Label>
                        </td>
                        <td style="text-align: left; width: 30%">
                            <asp:TextBox ID="Txt_Colonia_Notificacion" runat="server"></asp:TextBox>
                            <asp:HiddenField ID="Hdn_Colonia_ID_Notificacion" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%">
                            &nbsp;
                        </td>
                        <td style="text-align: left; width: 10%">
                            <asp:Label ID="Label7" runat="server" Text="Interior"></asp:Label>
                        </td>
                        <td style="text-align: left; width: 30%">
                            <asp:TextBox ID="Txt_Interior_Notificacion" runat="server"></asp:TextBox>
                        </td>
                        <td style="text-align: left; width: 10%">
                            <asp:Label ID="Label8" runat="server" Text="Exterior"></asp:Label>
                        </td>
                        <td style="text-align: left; width: 30%">
                            <asp:TextBox ID="Txt_Exterior_Notificacion" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%">
                            Por Rezago
                        </td>
                        <td style="text-align: left; width: 10%">
                            <asp:RadioButton ID="Btn_Por_Rezago" runat="server" GroupName="Tipo_Adeudo" />
                        </td>
                        <td style="text-align: left; width: 30%">
                        </td>
                        <td style="text-align: left; width: 10%">
                        </td>
                        <td style="text-align: left; width: 30%">
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%">
                            Por Corriente
                        </td>
                        <td style="text-align: left; width: 10%">
                            <asp:RadioButton ID="Btn_Por_Corriente" runat="server" GroupName="Tipo_Adeudo" />
                        </td>
                        <td style="text-align: left; width: 30%">
                        </td>
                        <td style="text-align: left; width: 10%">
                        </td>
                        <td style="text-align: left; width: 30%">
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%">
                            Periodo Final</td>
                        <td colspan="2" style="text-align: left; ">
                            <asp:DropDownList ID="Cmb_Años" runat="server">
                            </asp:DropDownList>
                            <asp:DropDownList ID="Cmb_Bimestres" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td style="text-align: left; width: 10%">
                            &nbsp;</td>
                        <td style="text-align: left; width: 30%">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="5">
                        </td>
                    </tr>
                </table>
                <table width="98%" class="estilo_fuente">
                    <tr>
                        <td style="text-align: left; width: 25%">
                            Monto de Rezago Inicial
                        </td>
                        <td style="text-align: left; width: 25%">
                            <asp:TextBox ID="Txt_Monto_Rezago_Inicial" runat="server" Width="84%" TabIndex="12"
                                MaxLength="11" Height="18px" />
                        </td>
                        <td style="text-align: right; width: 25%">
                            Monto Rezago Final
                        </td>
                        <td style="text-align: left; width: 25%">
                            <asp:TextBox ID="Txt_Monto_Rezago_Final" runat="server" Height="18px" MaxLength="11"
                                TabIndex="12" Width="84%" />
                        </td>
                    </tr>
                </table>
                <br />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

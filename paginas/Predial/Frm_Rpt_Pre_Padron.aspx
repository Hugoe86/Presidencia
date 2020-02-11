<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Rpt_Pre_Padron.aspx.cs" Inherits="paginas_Predial_Frm_Rpt_Pre_Padron" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
     <script type="text/javascript" language="javascript">
        //Abrir una ventana modal
		function Abrir_Ventana_Modal(Url, Propiedades)
		{
			window.showModalDialog(Url, null, Propiedades);
		}
     </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
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
                            Reporte de Padrón
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
                                            <asp:ImageButton ID="Btn_Exportar_pdf" runat="server" 
                                                AlternateText="Exportar a pdf" CssClass="Img_Button" 
                                                ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png" 
                                                TabIndex="1" ToolTip="Exportar a pdf" onclick="Btn_Exportar_pdf_Click" />
                                            <asp:ImageButton ID="Btn_Exportar_Excel" runat="server" 
                                                AlternateText="Exportar a Excel" CssClass="Img_Button" 
                                                ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png" 
                                                TabIndex="1" ToolTip="Exportar a Excel" 
                                                onclick="Btn_Exportar_Excel_Click" />
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                                TabIndex="5" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
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
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 25%">
                            Cuenta Predial
                        </td>
                        <td style="text-align: left; width: 5%">
                            <asp:RadioButton ID="Opt_Cuenta" runat="server"
                                GroupName="Reporte_Cuentas" />
                        </td>
                        <td style="text-align: right; width: 35%" colspan="2">
                            <asp:TextBox ID="Txt_Cuenta_Predial" runat="server" Width="98%"></asp:TextBox>
                        </td>                        
                    </tr>                    
                    <tr>
                        <td style="text-align: left; width: 25%">
                            Nombre de Contribuyente
                        </td>
                        <td style="text-align: left; width: 5%">
                            <asp:RadioButton ID="Opt_Nombre_Contribuyente" runat="server"
                                GroupName="Reporte_Cuentas" />
                        </td>
                        <td style="text-align: right; width: 70%" colspan="2">
                            <asp:TextBox ID="Txt_Nombre_Contribuyente" runat="server" Width="98%"></asp:TextBox>
                        </td>
                    </tr>
                    
                    <%--<tr>
                        <td style="text-align: left; width: 25%">
                            Ubicacion</td>
                        <td style="text-align:left; width: 5%">
                            <asp:RadioButton ID="Opt_Ubicacion" runat="server" 
                                GroupName="Reporte_Cuentas" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Calle</td>
                        <td style="text-align: center; width: 25%">
                            <asp:TextBox ID="Txt_Calle" runat="server" Width="93%"></asp:TextBox>
                        </td>                        
                        <td style="text-align: right;text-align: left; width:25%;">                            
                            Colonia&nbsp;&nbsp;
                            <asp:TextBox ID="Txt_Colonia" runat="server" Width="80%"></asp:TextBox>&nbsp;&nbsp;
                            <asp:ImageButton ID="Btn_Seleccionar_Colonia" runat="server" Height="22px" 
                                ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" 
                                TabIndex="10" 
                                ToolTip="Seleccionar Calle y Colonia" Width="22px" />
                        </td>                        
                    </tr>--%>
                    <tr>
                        <td style="text-align: left;" colspan="4">
                        </td>
                    </tr>
                </table>                
                <br />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


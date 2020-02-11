<%@ Page Language="C#" AutoEventWireup="true"MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
     CodeFile="Frm_Cat_Tra_Parametros.aspx.cs" Inherits="paginas_Tramites_Frm_Cat_Tra_Parametros" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">

    <script language="javascript" type="text/javascript">
    <!--
        //Abrir una ventana modal
        function Abrir_Ventana_Modal(Url, Propiedades)
        {
            window.showModalDialog(Url, null, Propiedades);
        }     
    //-->
    </script>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">

    <cc1:ToolkitScriptManager ID="Tsm_Parametros_Ordenamiento" runat="server" AsyncPostBackTimeout="3600"
        EnableScriptGlobalization="true" EnableScriptLocalization="true" />
    <div id="Div_Principal" style="background-color: #ffffff; width: 100%; height: 100%;">
        <asp:UpdatePanel ID="Upd_Panel" runat="server">
            <ContentTemplate>
                <%--<asp:UpdateProgress ID="Uprg_Servicios" runat="server" AssociatedUpdatePanelID="Upd_Panel"
                    DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                        </div>
                        <div class="processMessage" id="div_progress">
                            <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                    </ProgressTemplate>
                </asp:UpdateProgress>--%>
                <table width="99.5%" border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo" colspan="4">
                            Parámetros de Tramites
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Image ID="Img_Informacion" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" />
                            <asp:Label ID="Lbl_Informacion" runat="server" ForeColor="Red" TabIndex="0" Text="Mensajes de advertencia"></asp:Label>
                        </td>
                    </tr>
                </table>
                <div id="Div_Barra_Herramientas" runat="server">
                    <table width="99.5%" border="0" cellspacing="0" class="estilo_fuente">
                        <tr class="barra_busqueda">
                            <td colspan="2">
                                <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                    CssClass="Img_Button" AlternateText="Modificar" ToolTip="Modificar" OnClick="Btn_Modificar_Click" />
                                <asp:ImageButton ID="Btn_Salir" runat="server" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                    ToolTip="Inicio" OnClick="Btn_Salir_Click" />
                            </td>
                            <td colspan="2" align="right">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
                
                <div id="Div_Contenido1" runat="server" style="width: 99%;">
                    <table width="99.5%" border="0" cellspacing="0">
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%;">
                                *&nbsp;Encabezado del correo
                            </td>
                            <td style="width: 80%; text-align: right;">
                                <asp:TextBox ID="Txt_Correo_Encabezado" runat="server" TextMode="MultiLine" Rows="4" Width="90%" MaxLength="2000"></asp:TextBox>
                                    <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Correo_Encabezado" runat="server" 
                                        WatermarkText="   Limite de 2000 Caracteres  " TargetControlID="Txt_Correo_Encabezado" 
                                        WatermarkCssClass="watermarked"></cc1:TextBoxWatermarkExtender>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%;">
                                *&nbsp;Cuerpo del correo
                            </td>
                            <td style="width: 80%; text-align: right;">
                                <asp:TextBox ID="Txt_Correo_Cuerpo" runat="server" TextMode="MultiLine" Rows="4" Width="90%" MaxLength="2000"></asp:TextBox>
                                    <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Correo_Cuerpo" runat="server" 
                                        WatermarkText="   Limite de 2000 Caracteres  " TargetControlID="Txt_Correo_Cuerpo" 
                                        WatermarkCssClass="watermarked"></cc1:TextBoxWatermarkExtender>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%;">
                                *&nbsp;Despedida del correo
                            </td>
                            <td style="width: 80%; text-align: right;">
                                <asp:TextBox ID="Txt_Correo_Despedida" runat="server" TextMode="MultiLine" Rows="4" Width="90%" MaxLength="2000"></asp:TextBox>
                                     <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Correo_Despedida" runat="server" 
                                        WatermarkText="   Limite de 2000 Caracteres  " TargetControlID="Txt_Correo_Despedida" 
                                        WatermarkCssClass="watermarked"></cc1:TextBoxWatermarkExtender>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%;">
                                *&nbsp;Firma del correo
                            </td>
                            <td style="width: 80%; text-align: right;">
                                <asp:TextBox ID="Txt_Correo_Firma" runat="server" TextMode="MultiLine" Rows="4" Width="90%" MaxLength="2000"></asp:TextBox>
                                    <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Correo_Firma" runat="server" 
                                        WatermarkText="   Limite de 2000 Caracteres  " TargetControlID="Txt_Correo_Firma" 
                                        WatermarkCssClass="watermarked"></cc1:TextBoxWatermarkExtender>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

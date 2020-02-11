<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Ope_Cat_Cambiar_Fechas_Avaluos.aspx.cs" Inherits="paginas_Catastro_Frm_Ope_Cat_Cambiar_Fechas_Avaluos" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" Title="Digitalizacion de Avalúo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">
    <style type="text/css">
        body
        {
            font: normal 12px auto "Trebuchet MS" , Verdana;
            background-color: #ffffff;
            color: #4f6b72;
        }
        .link
        {
            color: Black;
        }
        .Label
        {
            width: 163px;
        }
        .TextBox
        {
            text-align: right;
        }
        a.enlace_fotografia:link, a.enlace_fotografia:visited
        {
            color: #25406D;
            text-decoration: underline;
            font-weight: normal;
            padding:0 5px 0 5px;
        }
        a.enlace_fotografia:hover
        {
            color: #25406D;
            text-decoration: underline;
            font-weight: bold;
            padding:0 5px 0 5px;
        }
        .style1
        {
            width: 239px;
        }
        .style2
        {
            width: 39%;
        }
    </style>
    <!--SCRIPT PARA LA VALIDACION QUE NO EXPIRE LA SESSION-->

    <script type="text/javascript" language="javascript">
        //El nombre del controlador que mantiene la sesión
        var CONTROLADOR = "../../Mantenedor_sesiones.ashx";

        //Ejecuta el script en segundo plano evitando así que caduque la sesión de esta página
        function MantenSesion()
        {
            var head = document.getElementsByTagName('head').item(0);
            script = document.createElement('script');
            script.src = CONTROLADOR;
            script.setAttribute('type', 'text/javascript');
            script.defer = true;
            head.appendChild(script);
        }

        //Temporizador para matener la sesión activa
//        setInterval('MantenSesion()', <%=(int)(0.9*(Session.Timeout * 60000))%>);

        window.onerror = new Function("return true");
        //Abrir una ventana modal
        function Abrir_Ventana_Modal(Url, Propiedades)
        {
            window.showModalDialog(Url, null, Propiedades);
        }

        function Abrir_Resumen(Url, Propiedades)
        {
            window.open(Url, 'Resumen_Predio', Propiedades);
        }
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <cc1:ToolkitScriptManager ID="Tool_ScriptManager" runat="server" EnableScriptGlobalization="true">
    </cc1:ToolkitScriptManager>
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel"
                DisplayAfter="0">
                <ProgressTemplate>
                   <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                    </div>
                    <div class="processMessage" id="div_progress">
                        <img alt="" src="../Imagenes/paginas/Updating.gif" />
                    </div>
					</ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Contenido" style="background-color: #ffffff; width: 100%; height: 100%;">
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo" >
                             Cambio De Fecha Para Avaluos
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png"
                                Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Encabezado_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error"></asp:Label>
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
                                        <td align="left" style="width: 59%;">
                                            <asp:ImageButton ID="Btn_Modificar" runat="server" TabIndex="3"
                                ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" Width="24px" CssClass="Img_Button"
                                AlternateText="Modificar" onclick = "Btn_Modificar_Click"  />
                                           
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Salir_Click" AlternateText="Salir" TabIndex="2"/>
                                        </td>
                                        <td align="right" style="width: 41%;">
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
                <br />
                
                <div id="Div_Detalles" runat="server" visible="true">
                    <table width="98%" class="estilo_fuente">
                           <%-- <tr style="background-color: #36C;">
                                <td colspan="4" style="text-align: left; font-size: 15px; color: #FFF;">
                                    Registros
                                </td>
                            </tr>--%>
                            <tr ID="Tr_Fila_Fotografias_Bien" runat="server" >
                                <td style="text-align: left;width:20% " class="style1">
                                   Folio Inicial
                                </td>
                                <td style="text-align: left; width:30%" class="style1" >
                                
                          
                            
                            
                                    <asp:TextBox ID="Txt_Folio_Inicial" runat="server"  Enabled="false" Width="85%" TabIndex="15" MaxLength="10"/>
                                    <cc1:FilteredTextBoxExtender ID="FTB_Txt_Folio_Inicial" runat="server" FilterType="Numbers" TargetControlID="Txt_Folio_Inicial" ValidChars="1234567890"/>
                                    
                                </td>
                                <td style="text-align: left; width:20%" class="style2">
                                    Folio Final
                                    </td>
                                    <td style="width:30%" class="style2">
                                    <asp:TextBox ID="Txt_Folio_Final" runat="server" FilterType="Numbers" Enabled="false" Width="85%" TabIndex="15" MaxLength="10"/>
                                    <cc1:FilteredTextBoxExtender ID="FTB_Txt_Folio_Final" runat="server" FilterType="Numbers" TargetControlID="Txt_Folio_Final" ValidChars="1234567890"/>
                                </td>
                            </tr>
                            <tr ID="Tr1" runat="server" >
                                <td style="text-align: left;width:20% " class="style1">
                                   Año
                                </td>
                                <td style="text-align: left; width:30%" class="style1" >
                                    <asp:TextBox ID="Txt_Anio" runat="server" FilterType="Numbers" Enabled="false" Width="85%" TabIndex="15" MaxLength="4"/>
                                    <cc1:FilteredTextBoxExtender ID="FTB_Anio"  runat="server" FilterType="Numbers" TargetControlID="Txt_Anio" ValidChars="1234567890"/>
                                    
                                </td>
                                <td style="text-align: left; width:20%" class="style2">
                                    Fecha a aplicar
                                    </td>
                                    <td style="width:30%; text-align:left;">
                            <asp:TextBox  runat="server" ID="Txt_Fecha_Avaluo" Enabled="false" Width="85%" TabIndex="6"
                                Style="float: left"/>
                                <cc1:TextBoxWatermarkExtender ID="Twe_Fecha_Avaluo" runat="server" 
                                TargetControlID="Txt_Fecha_Avaluo" WatermarkCssClass="watermarked" 
                                WatermarkText="Dia/Mes/Año" Enabled="True" />
                                <asp:ImageButton ID="Btn_Fecha_Avaluo" runat="server" Enabled="false"
                                ImageUrl="../imagenes/paginas/SmallCalendar.gif" style="vertical-align:top;"
                                Height="18px"/>
                            <cc1:CalendarExtender ID="Dtp_Fecha_Avaluo" runat="server" TargetControlID="Txt_Fecha_Avaluo" 
                                PopupButtonID="Btn_Fecha_Avaluo" Format="dd/MMM/yyyy">
                                </cc1:CalendarExtender>
                        </td>
                            </tr>
                            <tr>
                            <td>
                            <asp:HiddenField ID="Hdf_Cuenta_Predial_Id" runat="server" />
                            <asp:HiddenField ID="Hdf_Cuenta_Predial" runat="server" />
                            </td>
                            </tr>
                            <tr>
                            <td style="width:20%"></td>
                            <td style="width:30%"></td>
                            <td style="width:20%"></td>
                            <td style="width:30%">&nbsp;</td>
                            </tr>
                            
                        <%--<tr style="background-color: #36C;">
                                <td colspan="4" style="text-align: left; font-size: 15px; color: #FFF;">
                                    Observaciones al Perito
                                </td>
                            </tr>
                        <tr>
                            <td style="text-align: left; width: 20%; vertical-align: top;">
                                Observaciones
                            </td>
                            <td colspan="3" style="text-align: left; width: 80%; vertical-align: top;">
                                <asp:TextBox ID="Txt_Observaciones" runat="server" MaxLength="250" 
                                    Style="text-transform: uppercase;" TextMode="MultiLine" Width="98.6%" Enabled="false"/>
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Observaciones" runat="server" 
                                    TargetControlID="Txt_Observaciones" WatermarkCssClass="watermarked" 
                                    WatermarkText="Límite de Caracteres 250" />
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Observaciones" runat="server" 
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                    TargetControlID="Txt_Observaciones" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " />
                            </td>
                            </tr>--%>
                    </table>
                </div>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
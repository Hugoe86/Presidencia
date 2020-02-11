<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Cat_Cat_Calendario_Entregas.aspx.cs" 
Inherits="paginas_Catastro_Frm_Cat_Cat_Calendario_Entregas" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    Title="Catálogo de Cuotas para Peritos"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style1
        {
            width: 277px;
        }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">

    <script type='text/javascript'>
        function Abrir_Ventana_Modal(Url, Propiedades) {
            window.showModalDialog(Url, null, Propiedades);
        }
         function Abrir_Busqueda_Peritos_Internos() {
        $find('Busqueda_Peritos_Internos').show();
        return false;
    }
       // <!--
        //El nombre del controlador que mantiene la sesión
        var CONTROLADOR = "../../Mantenedor_Session.ashx";
 
        //Ejecuta el script en segundo plano evitando así que caduque la sesión de esta página
        function MantenSesion() {
            var head = document.getElementsByTagName('head').item(0);
            script = document.createElement('script');
            script.src = CONTROLADOR;
            script.setAttribute('type', 'text/javascript');
            script.defer = true;
            head.appendChild(script);
        }
 
        //Temporizador para matener la sesión activa
        setInterval("MantenSesion()", <%=(int)(0.9*(Session.Timeout * 60000))%>);
        
    //-->

    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <cc1:ToolkitScriptManager ID="Tsm_Generar_Nomina" runat="server" AsyncPostBackTimeout="3600"
        EnableScriptGlobalization="true" EnableScriptLocalization="true" />
    <asp:UpdatePanel ID="Upd_Otros_Pagos" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Otros_Pagos"
                DisplayAfter="0">
                <ProgressTemplate><%--
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                --%></ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Calles" style="background-color: #ffffff; width: 100%; height: 100%;">
                <table width="98%" border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo">
                            Calendario De Fechas De Entrega
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
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr class="barra_busqueda" align="right">
                        <td align="left" class="style1">
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" TabIndex="1" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"
                                Width="24px" CssClass="Img_Button" AlternateText="Nuevo" OnClick="Btn_Nuevo_Click" />
                            <asp:ImageButton ID="Btn_Modificar" runat="server" TabIndex="2" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                Width="24px" CssClass="Img_Button" AlternateText="Modificar" OnClick="Btn_Modificar_Click" />
                            <asp:ImageButton ID="Btn_Salir" runat="server" TabIndex="4" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                Width="24px" CssClass="Img_Button" AlternateText="Salir" OnClick="Btn_Salir_Click" />
                        </td>
                    </tr>
                </table>
                <br />
                <center>
                    <table width="98%" class="estilo_fuente">
                        <div id="Div_Grid_Cuotas_Perito" runat="server" visible="true">                         
                                <asp:HiddenField ID="Hdf_Fecha_Id" runat="server" /> 
                                <tr>
                                    <td style="text-align: left; width: 20%; text-align: left;">
                                        *Año
                                    </td>
                                    <td style="text-align: left; width: 30%; text-align: left;">
                                        <asp:TextBox ID="Txt_Anio" runat="server" Enabled="false" MaxLength="4" 
                                            Style="float: left; text-transform: uppercase" Width="94.4%" />
                                        <cc1:FilteredTextBoxExtender ID="FTB_Txt_Anio" runat="server" 
                                            FilterType="Numbers" TargetControlID="Txt_Anio" />
                                    </td>
                            </tr>
                            <tr style="background-color: #3366CC">
                                <td style="text-align: left; font-size: 15px; color: #FFFFFF;" colspan="4">
                                   Calendario  Entregas
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 20%; text-align: left;">
                                    * Fecha Primera Entrega                               
                                </td>
                                <td style="text-align: right; width: 30%; text-align: right;">
                                    <asp:TextBox ID="Txt_1_Entrega" runat="server" Width="94.4%" Style="float: left;
                                        text-transform: uppercase" MaxLength="24" Enabled="false"/>
                                    <cc1:TextBoxWatermarkExtender ID="Twe_Txt_1_Entrega" runat="server" TargetControlID="Txt_1_Entrega"
                                        WatermarkCssClass="watermarked" WatermarkText="Dia/Mes/Año" Enabled="True" />
                                    <asp:ImageButton ID="Btn_Calcula_1_Entrega" runat="server" 
                                        ImageUrl="../imagenes/paginas/SmallCalendar.gif" Style="vertical-align: top;" Height="18px" />
                                    <cc1:CalendarExtender ID="Dtp_Fecha_1_Entrega" runat="server" TargetControlID="Txt_1_Entrega"
                                        PopupButtonID="Btn_Calcula_1_Entrega" Format="dd/MMM/yyyy">
                                </cc1:CalendarExtender> 
                                </td>
                                <td style="text-align: right; width: 20%; text-align: left;">
                                    * Fecha Primera Entrega Real
                                </td>
                                <td style="text-align: right; width: 30%; text-align: right;">
                                    <asp:TextBox ID="Txt_1_Entrega_Real" runat="server" Width="94.4%" Style="float: left;
                                        text-transform: uppercase" MaxLength="24" Enabled="false"/>
                                    <cc1:TextBoxWatermarkExtender ID="Twe_Txt_1_Entrega_Real" runat="server" TargetControlID="Txt_1_Entrega_Real"
                                        WatermarkCssClass="watermarked" WatermarkText="Dia/Mes/Año" Enabled="True" />
                                    <asp:ImageButton ID="Btn_Calcula_1_Entrega_Real" runat="server" 
                                        ImageUrl="../imagenes/paginas/SmallCalendar.gif" Style="vertical-align: top;" Height="18px" />
                                    <cc1:CalendarExtender ID="Dtp_Fecha_1_Entrega_Real" runat="server" TargetControlID="Txt_1_Entrega_Real"
                                        PopupButtonID="Btn_Calcula_1_Entrega_Real" Format="dd/MMM/yyyy">
                                    </cc1:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 20%; text-align: left;">
                                    * Fecha Segunda Entrega
                                </td>
                                <td style="text-align: right; width: 30%; text-align: right;">
                                    <asp:TextBox ID="Txt_2_Entrega" runat="server" Width="94.4%" Style="float: left;
                                        text-transform: uppercase" MaxLength="24" Enabled="false"/>
                                    <cc1:TextBoxWatermarkExtender ID="Twe_Txt_2_Entrega" runat="server" TargetControlID="Txt_2_Entrega"
                                        WatermarkCssClass="watermarked" WatermarkText="Dia/Mes/Año" Enabled="True" />
                                    <asp:ImageButton ID="Btn_Calcula_2_Entrega" runat="server" ToolTip="Calcular" 
                                        ImageUrl="../imagenes/paginas/SmallCalendar.gif" Style="vertical-align: top;" Height="18px"/>
                                    <cc1:CalendarExtender ID="Dtp_Txt_2_Entrega" runat="server" TargetControlID="Txt_2_Entrega"
                                        PopupButtonID="Btn_Calcula_2_Entrega" Format="dd/MMM/yyyy">
                                    </cc1:CalendarExtender>
                                </td>
                                <td style="text-align: right; width: 20%; text-align: left;">
                                    * Fecha Segunda Entrega Real
                                </td>
                                <td style="text-align: right; width: 30%; text-align: right;">
                                    <asp:TextBox ID="Txt_2_Entrega_Real" runat="server" Width="94.4%" Style="float: left;
                                        text-transform: uppercase" MaxLength="24" Enabled="false"/>
                                    <cc1:TextBoxWatermarkExtender ID="Twe_Txt_2_Entrega_Real" runat="server" TargetControlID="Txt_2_Entrega_Real"
                                        WatermarkCssClass="watermarked" WatermarkText="Dia/Mes/Año" Enabled="True" />
                                    <asp:ImageButton ID="Btn_Calcula_2_Entrega_Real" runat="server" ToolTip="Calcular" 
                                        ImageUrl="../imagenes/paginas/SmallCalendar.gif" Style="vertical-align: top;" Height="18px" />
                                    <cc1:CalendarExtender ID="Dtp_Txt_2_Entrega_Real" runat="server" TargetControlID="Txt_2_Entrega_Real"
                                        PopupButtonID="Btn_Calcula_2_Entrega_Real" Format="dd/MMM/yyyy">
                                    </cc1:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 20%; text-align: left;">
                                    * Fecha Tercera Entrega
                                </td>
                                <td style="text-align: right; width: 30%; text-align: right;">
                                    <asp:TextBox ID="Txt_3_Entrega" runat="server" Width="94.4%" Style="float: left;
                                        text-transform: uppercase" MaxLength="24" Enabled="false"/>
                                    <cc1:TextBoxWatermarkExtender ID="Twe_Txt_3_Entrega" runat="server" TargetControlID="Txt_3_Entrega"
                                        WatermarkCssClass="watermarked" WatermarkText="Dia/Mes/Año" Enabled="True" />
                                    <asp:ImageButton ID="Btn_Calcula_3_Entrega" runat="server" ToolTip="Calcular" 
                                        ImageUrl="../imagenes/paginas/SmallCalendar.gif" Style="vertical-align: top;" Height="18px" />
                                    <cc1:CalendarExtender ID="Dtp_Txt_3_Entrega" runat="server" TargetControlID="Txt_3_Entrega"
                                        PopupButtonID="Btn_Calcula_3_Entrega" Format="dd/MMM/yyyy">
                                    </cc1:CalendarExtender>    
                                </td>
                                <td style="text-align: right; width: 20%; text-align: left;">
                                    * Fecha Tercera Entrega Real
                                </td>
                                <td style="text-align: right; width: 30%; text-align: right;">
                                    <asp:TextBox ID="Txt_3_Entrega_Real" runat="server" Width="94.4%" Style="float: left;
                                        text-transform: uppercase" MaxLength="24" Enabled="false"/>
                                    <cc1:TextBoxWatermarkExtender ID="Twe_Txt_3_Entrega_Real" runat="server" TargetControlID="Txt_3_Entrega_Real"
                                        WatermarkCssClass="watermarked" WatermarkText="Dia/Mes/Año" Enabled="True" />
                                    <asp:ImageButton ID="Btn_Calcula_3_Entrega_Real" runat="server" ToolTip="Calcular" 
                                        ImageUrl="../imagenes/paginas/SmallCalendar.gif" Style="vertical-align: top;" Height="18px" />
                                    <cc1:CalendarExtender ID="Dtp_Txt_3_Entrega_Real" runat="server" TargetControlID="Txt_3_Entrega_Real"
                                        PopupButtonID="Btn_Calcula_3_Entrega_Real" Format="dd/MMM/yyyy">
                                    </cc1:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 20%; text-align: left;">
                                    * Fecha Cuarta Entrega
                                </td>
                                <td style="text-align: right; width: 30%; text-align: right;">
                                    <asp:TextBox ID="Txt_4_Entrega" runat="server" Width="94.4%" Style="float: left;
                                        text-transform: uppercase" MaxLength="24" Enabled="false"/>
                                    <cc1:TextBoxWatermarkExtender ID="Twe_Txt_4_Entrega" runat="server" TargetControlID="Txt_4_Entrega"
                                        WatermarkCssClass="watermarked" WatermarkText="Dia/Mes/Año" Enabled="True" />
                                    <asp:ImageButton ID="Btn_Calcula_4_Entrega" runat="server" ToolTip="Calcular" 
                                        ImageUrl="../imagenes/paginas/SmallCalendar.gif" Style="vertical-align: top;" Height="18px" />
                                    <cc1:CalendarExtender ID="Dtp_Txt_4_Entrega" runat="server" TargetControlID="Txt_4_Entrega"
                                        PopupButtonID="Btn_Calcula_4_Entrega" Format="dd/MMM/yyyy">
                                    </cc1:CalendarExtender>    
                                </td>
                                <td style="text-align: right; width: 20%; text-align: left;">
                                    * Fecha Cuarta Entrega Real
                                    
                                </td>
                                <td style="text-align: right; width: 30%; text-align: right;">
                                    <asp:TextBox ID="Txt_4_Entrega_Real" runat="server" Width="94.4%" Style="float: left;
                                        text-transform: uppercase" MaxLength="25" Enabled="false"/>
                                    <cc1:TextBoxWatermarkExtender ID="Twe_Txt_4_Entrega_Real" runat="server" TargetControlID="Txt_4_Entrega_Real"
                                        WatermarkCssClass="watermarked" WatermarkText="Dia/Mes/Año" Enabled="True" />
                                    <asp:ImageButton ID="Btn_Calcula_4_Entrega_Real" runat="server" ToolTip="Calcular" 
                                        ImageUrl="../imagenes/paginas/SmallCalendar.gif" Style="vertical-align: top;" Height="18px" />
                                    <cc1:CalendarExtender ID="Dtp_Txt_4_Entrega_Real" runat="server" TargetControlID="Txt_4_Entrega_Real"
                                        PopupButtonID="Btn_Calcula_4_Entrega_Real" Format="dd/MMM/yyyy">
                                    </cc1:CalendarExtender>    
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 20%; text-align: left;">
                                    * Fecha Quinta Entrega
                                </td>
                                <td style="text-align: right; width: 30%; text-align: right;">
                                    <asp:TextBox ID="Txt_5_Entrega" runat="server" Width="94.4%" Style="float: left;
                                        text-transform: uppercase" MaxLength="24" Enabled="false"/>
                                    <cc1:TextBoxWatermarkExtender ID="Twe_Txt_5_Entrega" runat="server" TargetControlID="Txt_5_Entrega"
                                        WatermarkCssClass="watermarked" WatermarkText="Dia/Mes/Año" Enabled="True" />
                                    <asp:ImageButton ID="Btn_Calcula_5_Entrega" runat="server" ToolTip="Calcular" 
                                        ImageUrl="../imagenes/paginas/SmallCalendar.gif" Style="vertical-align: top;" Height="18px" /> 
                                    <cc1:CalendarExtender ID="Dtp_Txt_5_Entrega" runat="server" TargetControlID="Txt_5_Entrega"
                                        PopupButtonID="Btn_Calcula_5_Entrega" Format="dd/MMM/yyyy">
                                    </cc1:CalendarExtender> 
                                </td>
                                <td style="text-align: right; width: 20%; text-align: left;">
                                    * Fecha Quinta Entrega Real
                                </td>
                                <td style="text-align: right; width: 30%; text-align: right;">
                                    <asp:TextBox ID="Txt_5_Entrega_Real" runat="server" Width="94.4%" Style="float: left;
                                        text-transform: uppercase" MaxLength="25" Enabled="false"/>
                                     <cc1:TextBoxWatermarkExtender ID="Twe_Txt_5_Entrega_Real" runat="server" TargetControlID="Txt_5_Entrega_Real"
                                        WatermarkCssClass="watermarked" WatermarkText="Dia/Mes/Año" Enabled="True" />
                                    <asp:ImageButton ID="Btn_Calcula_5_Entrega_Real" runat="server" ToolTip="Calcular" 
                                        ImageUrl="../imagenes/paginas/SmallCalendar.gif" Style="vertical-align: top;" Height="18px" /> 
                                    <cc1:CalendarExtender ID="Dtp_Txt_5_Entrega_Real" runat="server" TargetControlID="Txt_5_Entrega_Real"
                                        PopupButtonID="Btn_Calcula_5_Entrega_Real" Format="dd/MMM/yyyy">
                                    </cc1:CalendarExtender>      
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 20%; text-align: left;">
                                    * Fecha Sexta Entrega
                                </td>
                                <td style="text-align: right; width: 30%; text-align: right;">
                                    <asp:TextBox ID="Txt_6_Entrega" runat="server" Width="94.4%" Style="float: left;
                                        text-transform: uppercase" MaxLength="24" Enabled="false"/>
                                    <cc1:TextBoxWatermarkExtender ID="Twe_Txt_6_Entrega" runat="server" TargetControlID="Txt_6_Entrega"
                                        WatermarkCssClass="watermarked" WatermarkText="Dia/Mes/Año" Enabled="True" />
                                    <asp:ImageButton ID="Btn_Calcula_6_Entrega" runat="server" ToolTip="Calcular" 
                                        ImageUrl="../imagenes/paginas/SmallCalendar.gif" Style="vertical-align: top;" Height="18px" /> 
                                    <cc1:CalendarExtender ID="Dtp_Txt_6_Entrega" runat="server" TargetControlID="Txt_6_Entrega"
                                        PopupButtonID="Btn_Calcula_6_Entrega" Format="dd/MMM/yyyy">
                                    </cc1:CalendarExtender> 
                                </td>
                                <td style="text-align: right; width: 20%; text-align: left;">
                                    * Fecha Sexta Entrega Real
                                </td>
                                <td style="text-align: right; width: 30%; text-align: right;">
                                    <asp:TextBox ID="Txt_6_Entrega_Real" runat="server" Width="94.4%" Style="float: left;
                                        text-transform: uppercase" MaxLength="25" Enabled="false"/>
                                    <cc1:TextBoxWatermarkExtender ID="Twe_Txt_6_Entrega_Real" runat="server" TargetControlID="Txt_6_Entrega_Real"
                                        WatermarkCssClass="watermarked" WatermarkText="Dia/Mes/Año" Enabled="True" />
                                    <asp:ImageButton ID="Btn_Calcula_6_Entrega_Real" runat="server" ToolTip="Calcular" 
                                       ImageUrl="../imagenes/paginas/SmallCalendar.gif" Style="vertical-align: top;" Height="18px" /> 
                                    <cc1:CalendarExtender ID="Dtp_Txt_6_Entrega_Real" runat="server" TargetControlID="Txt_6_Entrega_Real"
                                        PopupButtonID="Btn_Calcula_6_Entrega_Real" Format="dd/MMM/yyyy">
                                    </cc1:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 20%; text-align: left;">
                                    * Fecha Septima Entrega
                                </td>
                                <td style="text-align: right; width: 30%; text-align: right;">
                                    <asp:TextBox ID="Txt_7_Entrega" runat="server" Width="94.4%" Style="float: left;
                                        text-transform: uppercase" MaxLength="24" Enabled="false"/>
                                    <cc1:TextBoxWatermarkExtender ID="Twe_Txt_7_Entrega" runat="server" TargetControlID="Txt_7_Entrega"
                                        WatermarkCssClass="watermarked" WatermarkText="Dia/Mes/Año" Enabled="True" />
                                    <asp:ImageButton ID="Btn_Calcula_7_Entrega" runat="server" ToolTip="Calcular" 
                                        ImageUrl="../imagenes/paginas/SmallCalendar.gif" Style="vertical-align: top;" Height="18px" /> 
                                    <cc1:CalendarExtender ID="Dtp_Txt_7_Entrega" runat="server" TargetControlID="Txt_7_Entrega"
                                        PopupButtonID="Btn_Calcula_7_Entrega" Format="dd/MMM/yyyy">
                                    </cc1:CalendarExtender>
                                </td>
                                <td style="text-align: right; width: 20%; text-align: left;">
                                    * Fecha Septima Entrega Real
                                </td>
                                <td style="text-align: right; width: 30%; text-align: right;">
                                    <asp:TextBox ID="Txt_7_Entrega_Real" runat="server" Width="94.4%" Style="float: left;
                                        text-transform: uppercase" MaxLength="25" Enabled="false"/>
                                    <cc1:TextBoxWatermarkExtender ID="Twe_Txt_7_Entrega_Real" runat="server" TargetControlID="Txt_7_Entrega_Real"
                                        WatermarkCssClass="watermarked" WatermarkText="Dia/Mes/Año" Enabled="True" />
                                    <asp:ImageButton ID="Btn_Calcula_7_Entrega_Real" runat="server" ToolTip="Calcular" 
                                        ImageUrl="../imagenes/paginas/SmallCalendar.gif" Style="vertical-align: top;" Height="18px" /> 
                                    <cc1:CalendarExtender ID="Dtp_Txt_7_Entrega_Real" runat="server" TargetControlID="Txt_7_Entrega_Real"
                                        PopupButtonID="Btn_Calcula_7_Entrega_Real" Format="dd/MMM/yyyy">
                                    </cc1:CalendarExtender>
                                </td>
                            </tr>
                            
                            <tr>
                                <td colspan="4">
                                    <asp:GridView ID="Grid_Fecha_Entrega" runat="server" AllowPaging="True"   AllowSorting="True"
                                        AutoGenerateColumns="False" CssClass="GridView_1" EmptyDataText="&quot;No se encontraron registros&quot;"
                                        HeaderStyle-CssClass="tblHead" PageSize="10" Style="white-space: normal;" Width="100%"
                                        OnSelectedIndexChanged="Grid_Fecha_Entrega_SelectedIndexChanged" 
                                        OnPageIndexChanging="Grid_Fecha_Entrega_PageIndexChanging">
                                        <Columns>
                                            <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                            ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                <ItemStyle HorizontalAlign="Center" Width="3.5%" />
                                            </asp:ButtonField>
                                            <asp:BoundField DataField="FECHA_ENTREGA_ID" HeaderStyle-Width="5%" HeaderText="Id"
                                                Visible="false">
                                                <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                            </asp:BoundField>    
                                            <asp:BoundField DataField="ANIO" HeaderStyle-Width="5%" HeaderText="Año" >
                                                <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                            </asp:BoundField>                       
                                            <asp:BoundField DataField="FECHA_PRIMERA_ENTREGA" HeaderStyle-Width="5%" 
                                            HeaderText="Fecha 1a Entrega" DataFormatString="{0:dd-MMM-yyyy}">
                                                <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="FECHA_PRIMERA_ENTREGA_REAL" HeaderStyle-Width="10%" 
                                            HeaderText="Fecha 1a Entrega Real" DataFormatString="{0:dd-MMM-yyyy}">
                                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                <ItemStyle HorizontalAlign="Right" Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="FECHA_SEGUNDA_ENTREGA" HeaderStyle-Width="10%" 
                                            HeaderText="Fecha 2a Entrega" DataFormatString="{0:dd-MMM-yyyy}">
                                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                <ItemStyle HorizontalAlign="Right" Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="FECHA_SEGUNDA_ENTREGA_REAL" HeaderStyle-Width="10%" 
                                            HeaderText="Fecha 2a Entrega Real" DataFormatString="{0:dd-MMM-yyyy}">
                                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                <ItemStyle HorizontalAlign="Right" Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="FECHA_TERCERA_ENTREGA" HeaderStyle-Width="10%" 
                                            HeaderText="Fecha 3a Entrega" DataFormatString="{0:dd-MMM-yyyy}">
                                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                <ItemStyle HorizontalAlign="Right" Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="FECHA_TERCERA_ENTREGA_REAL" HeaderStyle-Width="10%" 
                                            HeaderText="Fecha 3a Entrega Real" DataFormatString="{0:dd-MMM-yyyy}">
                                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                <ItemStyle HorizontalAlign="Right" Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="FECHA_CUARTA_ENTREGA" HeaderStyle-Width="10%" 
                                            HeaderText="Fecha 4a Entrega" DataFormatString="{0:dd-MMM-yyyy}">
                                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                <ItemStyle HorizontalAlign="Right" Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="FECHA_CUARTA_ENTREGA_REAL" HeaderStyle-Width="10%" 
                                            HeaderText="Fecha 4a Entrega Real" DataFormatString="{0:dd-MMM-yyyy}">
                                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                <ItemStyle HorizontalAlign="Right" Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="FECHA_QUINTA_ENTREGA" HeaderStyle-Width="10%" 
                                            HeaderText="Fecha 5a Entrega" DataFormatString="{0:dd-MMM-yyyy}">
                                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                <ItemStyle HorizontalAlign="Right" Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="FECHA_QUINTA_ENTREGA_REAL" HeaderStyle-Width="10%" 
                                            HeaderText="Fecha 5a Entrega Real" DataFormatString="{0:dd-MMM-yyyy}">
                                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                <ItemStyle HorizontalAlign="Right" Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="FECHA_SEXTA_ENTREGA" HeaderStyle-Width="10%" 
                                            HeaderText="Fecha 6a Entrega" DataFormatString="{0:dd-MMM-yyyy}">
                                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                <ItemStyle HorizontalAlign="Right" Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="FECHA_SEXTA_ENTREGA_REAL" HeaderStyle-Width="10%" 
                                            HeaderText="Fecha 6a Entrega Real" DataFormatString="{0:dd-MMM-yyyy}">
                                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                <ItemStyle HorizontalAlign="Right" Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="FECHA_SEPTIMA_ENTREGA" HeaderStyle-Width="10%" 
                                            HeaderText="Fecha 7a Entrega" DataFormatString="{0:dd-MMM-yyyy}">
                                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                <ItemStyle HorizontalAlign="Right" Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="FECHA_SEPTIMA_ENTREGA_REAL" HeaderStyle-Width="10%" 
                                            HeaderText="Fecha 7a Entrega Real" DataFormatString="{0:dd-MMM-yyyy}">
                                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                <ItemStyle HorizontalAlign="Right" Width="10%" />
                                            </asp:BoundField>
                                        </Columns>
                                        <SelectedRowStyle CssClass="GridSelected" />
                                        <PagerStyle CssClass="GridHeader" />
                                        <HeaderStyle CssClass="tblHead" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                    </asp:GridView>
                                </td>
                            </tr>
                        </div>
                    </table>
            </div>
            </table> </center>            
        </ContentTemplate>
    </asp:UpdatePanel> 
</asp:Content>


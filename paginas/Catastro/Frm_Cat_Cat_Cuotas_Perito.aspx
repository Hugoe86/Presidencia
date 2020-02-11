<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Cat_Cat_Cuotas_Perito.aspx.cs"
    Inherits="paginas_Catastro_Frm_Cat_Cat_Cuotas_Perito" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    Title="Catálogo de Cuotas para Peritos" %>

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
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Calles" style="background-color: #ffffff; width: 100%; height: 100%;">
                <table width="98%" border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo">
                            Catálogo de Cuotas de Perito
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
                            <tr>
                                <td style="text-align: left; width: 20%; text-align: left;">
                                    Perito Interno
                                </td>
                                <td style="text-align: left; width: 20%; text-align: left;">
                                    <asp:TextBox ID="Txt_Perito_Interno" runat="server" Width="85%" Style="float: left; text-transform: uppercase"
                                        MaxLength="4" Enabled="false"/>
                                    
                                    <asp:ImageButton ID="Btn_Mostrar_Busqueda_Avanzada_Peritos_Internos" runat="server"
                                        Height="24px" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" OnClientClick="javascript:return Abrir_Busqueda_Peritos_Internos();"
                                        TabIndex="10" ToolTip="Búsqueda Avanzada de Peritos Internos" 
                                        Width="24px"/>
                                        <asp:HiddenField ID="Hdf_Perito_Interno_Id" runat="server" />
                                </td>
                                <td style="text-align: left; width: 20%; text-align: left;">
                                    *Año
                                </td>
                                <td style="text-align: left; width: 30%; text-align: left;">
                                    <asp:TextBox ID="Txt_Anio" runat="server" Width="94.4%" Style="float: left; text-transform: uppercase"
                                        MaxLength="4" Enabled="false"/>
                                    <cc1:FilteredTextBoxExtender FilterType="Numbers" runat="server" ID="FTB_Txt_Anio"
                                        TargetControlID="Txt_Anio" />
                                </td>
                            </tr>
                            <tr style="background-color: #3366CC">
                                <td style="text-align: left; font-size: 15px; color: #FFFFFF;" colspan="4">
                                    Cuota
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 20%; text-align: left;">
                                    * Primera Entrega
                                </td>
                                <td style="text-align: right; width: 20%; text-align: right;">
                                    <asp:TextBox ID="Txt_1_Entrega" runat="server" Width="94.4%" Style="float: left;
                                        text-transform: uppercase" MaxLength="24" Enabled="false"/>
                                    <cc1:FilteredTextBoxExtender FilterType="Numbers" runat="server" ID="FTB_Txt_1_Entrega"
                                        TargetControlID="Txt_1_Entrega" ValidChars="1234567890" />
                                </td>
                                <td style="text-align: right; width: 20%; text-align: left;">
                                    *Segunda Entrega
                                </td>
                                <td style="text-align: right; width: 20%; text-align: right;">
                                    <asp:TextBox ID="Txt_2_Entrega" runat="server" Width="94.4%" Style="float: left;
                                        text-transform: uppercase" MaxLength="24" Enabled="false"/>
                                    <cc1:FilteredTextBoxExtender FilterType="Numbers" runat="server" ID="FTB_Txt_2_Entrega"
                                        TargetControlID="Txt_2_Entrega" ValidChars="1234567890" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 20%; text-align: left;">
                                    *Tercera Entrega
                                </td>
                                <td style="text-align: right; width: 20%; text-align: right;">
                                    <asp:TextBox ID="Txt_3_Entrega" runat="server" Width="94.4%" Style="float: left;
                                        text-transform: uppercase" MaxLength="24" Enabled="false"/>
                                    <cc1:FilteredTextBoxExtender FilterType="Numbers" runat="server" ID="FTB_Txt_3_Entrega"
                                        TargetControlID="Txt_3_Entrega" ValidChars="1234567890" />
                                </td>
                                <td style="text-align: right; width: 20%; text-align: left;">
                                    *Cuarta Entrega
                                </td>
                                <td style="text-align: right; width: 20%; text-align: right;">
                                    <asp:TextBox ID="Txt_4_Entrega" runat="server" Width="94.4%" Style="float: left;
                                        text-transform: uppercase" MaxLength="24" Enabled="false"/>
                                    <cc1:FilteredTextBoxExtender FilterType="Numbers" runat="server" ID="FTB_Txt_4_Entrega"
                                        TargetControlID="Txt_4_Entrega" ValidChars="1234567890" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 20%; text-align: left;">
                                    *Quinta Entrega
                                </td>
                                <td style="text-align: right; width: 20%; text-align: right;">
                                    <asp:TextBox ID="Txt_5_Entrega" runat="server" Width="94.4%" Style="float: left;
                                        text-transform: uppercase" MaxLength="24" Enabled="false"/>
                                    <cc1:FilteredTextBoxExtender FilterType="Numbers" runat="server" ID="FTB_Txt_5_Entrega"
                                        TargetControlID="Txt_5_Entrega" ValidChars="1234567890" />
                                </td>
                                <td style="text-align: right; width: 20%; text-align: left;">
                                    *Sexta Entrega
                                </td>
                                <td style="text-align: right; width: 20%; text-align: right;">
                                    <asp:TextBox ID="Txt_6_Entrega" runat="server" Width="94.4%" Style="float: left;
                                        text-transform: uppercase" MaxLength="24" Enabled="false"/>
                                    <cc1:FilteredTextBoxExtender FilterType="Numbers" runat="server" ID="FTB_Txt_6_Entrega"
                                        TargetControlID="Txt_6_Entrega" ValidChars="1234567890" />
                                </td>
                            </tr>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 20%; text-align: left;">
                                    *Septima Entrega
                                </td>
                                <td style="text-align: right; width: 30%; text-align: right;">
                                    <asp:TextBox ID="Txt_7_Entrega" runat="server" Width="94.4%" Style="float: left;
                                        text-transform: uppercase" MaxLength="24" Enabled="false"/>
                                    <cc1:FilteredTextBoxExtender FilterType="Numbers" runat="server" ID="FTB_Txt_7_Entrega"
                                        TargetControlID="Txt_7_Entrega" ValidChars="1234567890" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:HiddenField ID="Hdf_Couta_Perito_Id" runat="server" />
                                    
                                    <%--<asp:HiddenField ID="Hdf_Cantidad_Cobro2" runat="server" />--%>
                                    <%--<asp:HiddenField ID="Hdf_Base_Cobro" runat="server" />--%>
                                </td>
                               
                                
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:GridView ID="Grid_Cuotas" runat="server" AllowPaging="True" AllowSorting="True"
                                        AutoGenerateColumns="False" CssClass="GridView_1" EmptyDataText="&quot;No se encontraron registros&quot;"
                                        HeaderStyle-CssClass="tblHead" PageSize="10" Style="white-space: normal;" Width="100%"
                                        OnSelectedIndexChanged="Grid_Cuotas_SelectedIndexChanged" OnPageIndexChanging="Grid_Cuotas_PageIndexChanging">
                                        <Columns>
                                            <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                <ItemStyle HorizontalAlign="Center" Width="3.5%" />
                                            </asp:ButtonField>
                                            <asp:BoundField DataField="CUOTA_PERITO_ID" HeaderStyle-Width="5%" HeaderText="Id"
                                                Visible="false">
                                                <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="PERITO_INTERNO_ID" HeaderStyle-Width="5%" HeaderText="Perito_Interno_Id"
                                                Visible="false">
                                                <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                                <ItemStyle HorizontalAlign="Left" Width="5%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ANIO" HeaderStyle-Width="5%" HeaderText="Año">
                                                <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="PRIMERA_ENTREGA" HeaderStyle-Width="10%" HeaderText="1a Entrega">
                                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                <ItemStyle HorizontalAlign="Right" Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SEGUNDA_Entrega" HeaderStyle-Width="10%" HeaderText="2a Entrega">
                                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                <ItemStyle HorizontalAlign="Right" Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="TERCERA_ENTREGA" HeaderStyle-Width="10%" HeaderText="3a Entrega">
                                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                <ItemStyle HorizontalAlign="Right" Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CUARTA_ENTREGA" HeaderStyle-Width="10%" HeaderText="4a Entrega">
                                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                <ItemStyle HorizontalAlign="Right" Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="QUINTA_ENTREGA" HeaderStyle-Width="10%" HeaderText="5a Entrega">
                                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                <ItemStyle HorizontalAlign="Right" Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SEXTA_ENTREGA" HeaderStyle-Width="10%" HeaderText="6a Entrega">
                                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                <ItemStyle HorizontalAlign="Right" Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SEPTIMA_ENTREGA" HeaderStyle-Width="10%" HeaderText="7a Entrega">
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
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
   <cc1:ModalPopupExtender ID="Mpe_Busqueda_Peritos_Internos" runat="server" TargetControlID="Btn_Comodin_Open_Busqueda_Peritos_Internos"
        PopupControlID="Pnl_Busqueda_Contenedor" BackgroundCssClass="popUpStyle" BehaviorID="Busqueda_Peritos_Internos"
        CancelControlID="Btn_Comodin_Close_Busqueda_Peritos_Internos" DropShadow="true"
        DynamicServicePath="" Enabled="True" />
    <asp:Button ID="Btn_Comodin_Close_Busqueda_Peritos_Internos" runat="server" Style="background-color: transparent;
        border-style: none; display: none;" Text="" />
    <asp:Button ID="Btn_Comodin_Open_Busqueda_Peritos_Internos" runat="server" Style="background-color: transparent;
        border-style: none; display: none;" Text="" />
    <%--Ventana modal de búsqueda de peritos internos--%>
    <asp:Panel ID="Pnl_Busqueda_Contenedor" runat="server" CssClass="drag" HorizontalAlign="Center"
        Width="650px" Style="display: none; border-style: outset; border-color: Silver;
        background-image: url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG); background-repeat: repeat-y;">
        <asp:Panel ID="Panel1" runat="server" Style="cursor: move; background-color: Silver;
            color: Black; font-size: 12; font-weight: bold; border-style: outset;">
            <table width="99%">
                <tr>
                    <td style="color: Black; font-size: 12; font-weight: bold;">
                        <asp:Image ID="Img_Informatcion_Autorizacion" runat="server" ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                        B&uacute;squeda: Peritos Internos
                    </td>
                    <td align="right" style="width: 10%;">
                        <asp:ImageButton ID="Btn_Cerrar_Ventana" CausesValidation="false" runat="server"
                            Style="cursor: pointer;" ToolTip="Cerrar Ventana" ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png"
                            OnClick="Btn_Cerrar_Ventana_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:UpdatePanel ID="Upnl_Filtros_Busqueda_Prestamos" runat="server">
            <ContentTemplate>
                <div style="color: #5D7B9D">
                    <table width="100%">
                        <tr>
                            <td align="left" style="text-align: left;">
                                <asp:UpdateProgress ID="Progress_Upnl_Filtros_Busqueda_Prestamos" runat="server"
                                    AssociatedUpdatePanelID="Upnl_Filtros_Busqueda_Prestamos" DisplayAfter="0">
                                    <ProgressTemplate>
                                        <div id="progressBackgroundFilter1" class="progressBackgroundFilter">
                                        </div>
                                        <div style="background-color: Transparent; position: fixed; top: 50%; left: 47%;
                                            padding: 10px; z-index: 1002;" id="div_progress1">
                                            <img alt="" src="../Imagenes/paginas/Sias_Roler.gif" /></div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                                <table width="100%">
                                    <%--                                   <tr>
                                        <td style="width:100%" colspan="4" align="right">
                                            <asp:ImageButton ID="Btn_Limpiar_Ctlr_Busqueda" runat="server" OnClientClick="javascript:return Limpiar_Ctlr();"
                                                ImageUrl="~/paginas/imagenes/paginas/sias_clear.png" ToolTip="Limpiar Controles de Busqueda"/>                         
                                        </td>
                                    </tr>     --%>
                                    <tr>
                                        <td style="width: 100%" colspan="4">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%; text-align: left; font-size: 11px;">
                                            Nombre Perito Interno
                                        </td>
                                        <td style="width: 30%; text-align: right; font-size: 11px;" colspan="3">
                                            <asp:TextBox ID="Txt_Busqueda_Nombre" runat="server" Width="98%" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:GridView ID="Grid_Peritos_Externos" runat="server" AllowPaging="True" AllowSorting="True"
                                                AutoGenerateColumns="False" CssClass="GridView_1" EmptyDataText="&quot;No se encontraron registros&quot;"
                                                HeaderStyle-CssClass="tblHead" PageSize="15" Style="white-space: normal;" Width="100%"
                                                OnSelectedIndexChanged="Grid_Peritos_Externos_SelectedIndexChanged" OnPageIndexChanging="Grid_Peritos_Externos_PageIndexChanging">
                                                <Columns>
                                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                        <ItemStyle HorizontalAlign="Center" Width="3.5%" />
                                                    </asp:ButtonField>
                                                    <asp:BoundField DataField="PERITO_INTERNO_ID" HeaderStyle-Width="15%" HeaderText="ID"
                                                        Visible="false">
                                                        <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="EMPLEADO_ID" HeaderStyle-Width="15%" HeaderText="ID_Empleado"
                                                        Visible="false">
                                                        <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="EMPLEADO" HeaderStyle-Width="15%" HeaderText="Perito Interno">
                                                        <HeaderStyle HorizontalAlign="Left" Width="70%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="70%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="RFC" HeaderText="RFC">
                                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <SelectedRowStyle CssClass="GridSelected" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <HeaderStyle CssClass="tblHead" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%; text-align: left;" colspan="4">
                                            <center>
                                                <asp:Button ID="Btn_Busqueda_Empleados" runat="server" Text="Busqueda Peritos Internos"
                                                    CssClass="button" CausesValidation="false" Width="200px" OnClick="Btn_Busqueda_Empleados_Click" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>

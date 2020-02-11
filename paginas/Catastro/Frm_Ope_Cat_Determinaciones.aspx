<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Ope_Cat_Determinaciones.aspx.cs" Inherits="paginas_Catastro_Frm_Ope_Cat_Determinaciones" 
MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" Title="Determinaciones"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">

    <script type='text/javascript'>

    <!--
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

    function Abrir_Busqueda_Colonias() {
        $find('Busqueda_Colonias').show();
        Window_Resize();
        return false;
    }

    function Abrir_Busqueda_Calles() {
        $find('Busqueda_Calles').show();
        Window_Resize();
        return false;
    }

    function Abrir_Ventana_Modal(Url, Propiedades)
	{
		window.showModalDialog(Url, null, Propiedades);
	}

    function ValidarCaracteres(textareaControl, maxlength) { if (textareaControl.value.length > maxlength) { textareaControl.value = textareaControl.value.substring(0, maxlength); alert("Debe ingresar hasta un máximo de " + maxlength + " caracteres"); } }

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <cc1:ToolkitScriptManager ID="Tsm_Generar_Nomina" runat="server" AsyncPostBackTimeout="3600"
        EnableScriptGlobalization="true" EnableScriptLocalization="true" />
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
                            Determinaciones
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="Div_Contenedor_Msj_Error" style="width: 98%;" runat="server" visible="false">
                                <table style="width: 100%;">
                                    <tr>
                                        <td colspan="2" align="left">
                                            <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png"
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
                </table>
                <table width="98%" border="0" cellspacing="0">
                    <tr align="center">
                        <td>
                            <div align="right" style="width: 99%; background-color: #2F4E7D; color: #FFFFFF;
                                font-weight: bold; font-style: normal; font-variant: normal; font-family: fantasy;
                                height: 32px">
                                <table style="width: 100%; height: 28px;">
                                    <tr>
                                        <td align="left" style="width: 59%;">
                                                <asp:ImageButton ID="Btn_Imprimir_Rep" runat="server" ToolTip="Reporte" CssClass="Img_Button"
                                                TabIndex="2" ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png" AlternateText="Reporte"
                                                OnClick="Btn_Imprime_Reporte_Click" />
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                                TabIndex="5" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" AlternateText="Salir"
                                                OnClick="Btn_Salir_Click" />
                                        </td>
                                        <td align="right" style="width: 41%;">
                                            <table style="width: 100%; height: 28px;">
                                                <tr>
                                                    <td style="vertical-align: middle; text-align: right; width: 20%;">
                                                        
                                                    </td>
                                                    <td style="width: 55%;">
                                                    </td>
                                                    <td style="vertical-align: middle; width: 5%;">
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
                <div id="Div_Total_Asignacion" runat="server">
                    <table width="98%" class="estilo_fuente">
                    <tr style="background-color: #3366CC">
                            <td style="text-align: left; font-size: 15px; color: #FFFFFF;" colspan="4">
                            Total de entregas
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Entrega
                            </td>
                            <td style="width: 30%; text-align: left;">
                                <asp:DropDownList ID="Cmb_Entrega" runat="server" Width="92%" AutoPostBack="true" OnSelectedIndexChanged="Cmb_Entrega_SelectedIndexChanged">
                                <asp:ListItem Text="1a. Entrega" Value="1_ENTREGA" />
                                <asp:ListItem Text="2a. Entrega" Value="2_ENTREGA" />
                                <asp:ListItem Text="3a. Entrega" Value="3_ENTREGA" />
                                <asp:ListItem Text="4a. Entrega" Value="4_ENTREGA" />
                                <asp:ListItem Text="5a. Entrega" Value="5_ENTREGA" />
                                <asp:ListItem Text="6a. Entrega" Value="6_ENTREGA" />
                                <asp:ListItem Text="7a. Entrega" Value="7_ENTREGA" />
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: left; width: 20%;">
                                Año
                            </td>
                            <td style="width: 30%; text-align: left;">
                                <asp:TextBox ID="Txt_Anio" runat="server" Width="98%" AutoPostBack="true" MaxLength="4" OnTextChanged="Txt_Anio_TextChanged"/>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Total de entregas predios urbanos
                            </td>
                            <td style="width: 30%; text-align: left;">
                                <asp:TextBox ID="Txt_Entregas_Urbanas" runat="server" Enabled="false" Width="98%" style="text-align:right"/>
                            </td>
                            <td style="text-align: left; width: 20%;">
                                Total de entregas de predios rústicos
                            </td>
                            <td style="width: 30%; text-align: left;">
                                <asp:TextBox ID="Txt_Entregas_Rusticas" runat="server" Enabled="false"  Width="98%" style="text-align:right"/>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Total de entregas predios del Municipio
                            </td>
                            <td style="width: 30%; text-align: left;">
                                <asp:TextBox ID="Txt_Entregas_Municipio" runat="server" Enabled="false"  Width="98%" style="text-align:right"/>
                            </td>
                            <td style="text-align: left; width: 20%;">
                                Total de entregas predios Estrategicos
                            </td>
                            <td style="width: 30%; text-align: left;">
                                <asp:TextBox ID="Txt_Entregas_Estrategicas" runat="server" Enabled="false"  Width="98%" style="text-align:right"/>
                            </td>
                        </tr>
                        </table>
                        </div>
                <div id="Div_Modificar" runat="server">
                    <table width="98%" class="estilo_fuente">
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                
                            </td>
                            <td style="width: 30%; text-align: left;">
                                
                            </td>
                            <td style="text-align: left; width: 20%;">
                                
                            </td>
                            <td style="width: 30%; text-align: left;">
                                
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <table width="98%" class="estilo_fuente">
                                    <tr>
                                        <td style="text-align: left;">
                                            <asp:GridView ID="Grid_Entregas" runat="server" AllowPaging="True" AllowSorting="True"
                                                AutoGenerateColumns="False" CssClass="GridView_1" EmptyDataText="&quot;No se encontraron registros&quot;"
                                                HeaderStyle-CssClass="tblHead" PageSize="100" Style="white-space: normal;" Width="100%"
                                                OnSelectedIndexChanged="Grid_Entregas_SelectedIndexChanged" OnPageIndexChanging="Grid_Entregas_PageIndexChanging">
                                                <Columns>
                                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                        <ItemStyle HorizontalAlign="Center" Width="3.5%" />
                                                    </asp:ButtonField>
                                                    <asp:BoundField DataField="NO_ASIGNACION" HeaderStyle-Width="15%" HeaderText="ID"
                                                        Visible="false">
                                                        <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ANIO" HeaderText="Año">
                                                        <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                                        <ItemStyle HorizontalAlign="Center" Width="8%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="NO_ENTREGA" HeaderText="Entrega">
                                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="CUENTA_PREDIAL" HeaderText="Cuenta Predial">
                                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="FOLIO_PREDIAL" HeaderText="Folio Predial">
                                                        <HeaderStyle HorizontalAlign="Center" Width="25%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="25%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="FOLIO_CATASTRO" HeaderText="Folio Catastro">
                                                        <HeaderStyle HorizontalAlign="Center" Width="25%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="25%" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <SelectedRowStyle CssClass="GridSelected" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <HeaderStyle CssClass="tblHead" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
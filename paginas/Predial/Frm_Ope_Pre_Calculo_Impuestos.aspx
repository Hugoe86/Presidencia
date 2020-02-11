<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Pre_Calculo_Impuestos.aspx.cs" Inherits="paginas_predial_Frm_Ope_Pre_Calculo_Impuestos"
    Title="Operación - Calculo de Impuesto" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style1
        {
            text-align: right;
        }
        .style2
        {
            text-align: right;
            width: 3%;
        }
        .style4
        {
            text-align: left;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">

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
        setInterval('MantenSesion()', "<%=(int)(0.9*(Session.Timeout * 60000))%>");

        function calendarShown(sender, args){
            sender._popupBehavior._element.style.zIndex = 10000005;
        }

		function getCookie(c_name)
		{
			if (document.cookie.length>0)
			{
			c_start=document.cookie.indexOf(c_name + "=");
			if (c_start!=-1)
				{
				c_start=c_start + c_name.length+1;
				c_end=document.cookie.indexOf(";",c_start);
				if (c_end==-1) c_end=document.cookie.length;
				return unescape(document.cookie.substring(c_start,c_end));
				}
			}
			return "";
		}

		function setCookie(c_name,value,expiredays)
		{
			var exdate=new Date();
			exdate.setDate(exdate.getDate()+expiredays);
			document.cookie=c_name+ "=" +escape(value)+
			((expiredays==null) ? "" : ";expires="+exdate.toUTCString());
		}
		//Abrir una ventana modal
		function Abrir_Ventana_Modal(Url, Propiedades)
		{
			window.showModalDialog(Url, null, Propiedades);
		}
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <asp:ScriptManager ID="ScptM_Calculo_Impuesto" runat="server" EnableScriptGlobalization="true"
        EnableScriptLocalization="True" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel"
                DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                    </div>
                    <div class="processMessage" id="div_progress">
                        <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Requisitos" style="background-color: #ffffff; width: 100%; height: 100%;">
                <table width="98%" border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo">
                            Cálculo Impuestos
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
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr class="barra_busqueda" align="right">
                        <td>
                            <asp:ImageButton ID="Btn_Calculo_Impuesto" runat="server" AlternateText="Modificar"
                                CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                OnClick="Btn_Calculo_Impuesto_Click" Width="24px" />
                            <asp:ImageButton ID="Btn_Calculo_Impuesto_Cancelar" runat="server" AlternateText="Salir"
                                CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Calculo_Impuesto_Cancelar_Click"
                                Width="24px" />
                        </td>
                    </tr>
                </table>
                <br />
                <cc1:TabContainer ID="Tab_Contenedor_Pestañas" runat="server" Width="98%" ActiveTabIndex="0"
                    Font-Bold="True">
                    <cc1:TabPanel runat="server" HeaderText="Tab_Panel_Contrarecibos_Listados" ID="Tab_Panel_Contrarecibos_Listados"
                        Width="100%">
                        <HeaderTemplate>
                            Contrarecibos y Listados</HeaderTemplate>
                        <ContentTemplate>
                            <cc1:TabContainer ID="Tab_Contenedor_Pestañas_Contrarecibos" runat="server" Width="98%"
                                ActiveTabIndex="1">
                                <cc1:TabPanel runat="server" HeaderText="Tab_Panel_Contrarecibos" ID="Tab_Panel_Contrarecibos"
                                    Width="100%">
                                    <HeaderTemplate>
                                        Contrarecibo</HeaderTemplate>
                                    <ContentTemplate>
                                        <div style="width: 98%; height: 150px;">
                                            <table width="100%">
                                                <tr>
                                                    <td style="text-align: left;" colspan="2">
                                                        <asp:Label ID="Lbl_Titulo_Busqueda" runat="server" Text="Búsqueda"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 20%; text-align: left;">
                                                        <asp:Label ID="Lbl_Cuenta_Predial" runat="server" Text="Cuenta Predial" CssClass="estilo_fuente"></asp:Label>
                                                    </td>
                                                    <td style="width: 80%; text-align: left;">
                                                        <asp:TextBox ID="Txt_Cuenta_Predial" runat="server" Width="200px"></asp:TextBox><cc1:FilteredTextBoxExtender
                                                            ID="FTE_Txt_Cuenta_Predial" runat="server" FilterType="Numbers, UppercaseLetters"
                                                            TargetControlID="Txt_Cuenta_Predial" Enabled="True">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 20%; text-align: left;">
                                                        <asp:Label ID="Lbl_No_Contrarecibo" runat="server" Text="No. Contrarecibo" CssClass="estilo_fuente"></asp:Label>
                                                    </td>
                                                    <td style="width: 80%; text-align: left;">
                                                        <asp:TextBox ID="Txt_No_Contrarecibo" runat="server" Width="200px"></asp:TextBox><cc1:FilteredTextBoxExtender
                                                            ID="FTE_Txt_No_Contrarecibo" runat="server" FilterType="Numbers" TargetControlID="Txt_No_Contrarecibo"
                                                            Enabled="True">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 20%; text-align: left;">
                                                        <asp:Label ID="Lbl_Fecha_Escritura" runat="server" Text="Fecha Escritura" CssClass="estilo_fuente"></asp:Label>
                                                    </td>
                                                    <td style="width: 80%; text-align: left;">
                                                        <asp:TextBox ID="Txt_Fecha_Escritura" runat="server" Width="200px" Enabled="False"></asp:TextBox><asp:ImageButton
                                                            ID="Btn_Fecha_Escritura" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                                            AlternateText="Seleccione la Fecha de Escritura" /><cc1:CalendarExtender ID="CE_Txt_Fecha_Fecha_Escritura"
                                                                runat="server" TargetControlID="Txt_Fecha_Escritura" PopupButtonID="Btn_Fecha_Escritura"
                                                                Format="dd/MMM/yyyy" Enabled="True">
                                                            </cc1:CalendarExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 20%; text-align: left;">
                                                        <asp:Label ID="Lbl_Fecha_Liberacion" runat="server" Text="Fecha Liberación" CssClass="estilo_fuente"></asp:Label>
                                                    </td>
                                                    <td style="width: 80%; text-align: left;">
                                                        <asp:TextBox ID="Txt_Fecha_Liberacion" runat="server" Width="200px" Enabled="False"></asp:TextBox><asp:ImageButton
                                                            ID="Btn_Fecha_Liberacion" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                                            AlternateText="Seleccione la Fecha de Liberación" /><cc1:CalendarExtender ID="CE_Txt_Fecha_Liberacion"
                                                                runat="server" TargetControlID="Txt_Fecha_Liberacion" PopupButtonID="Btn_Fecha_Liberacion"
                                                                Format="dd/MMM/yyyy" Enabled="True">
                                                            </cc1:CalendarExtender>
                                                        &#160;&#160;&#160;&#160;&#160;&#160;&#160;
                                                        <asp:ImageButton ID="Btn_Buscar_Contrarecibos" runat="server" CausesValidation="False"
                                                            ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Buscar Contrarecibos"
                                                            OnClick="Btn_Buscar_Contrarecibos_Click" /><asp:ImageButton ID="Btn_Limpiar_Filtros_Buscar_Contrarecibos"
                                                                runat="server" CausesValidation="False" ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png"
                                                                Width="20px" ToolTip="Limpiar Filtros" OnClick="Btn_Limpiar_Filtros_Buscar_Contrarecibos_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel runat="server" HeaderText="Tab_Panel_Listados" ID="Tab_Panel_Listados"
                                    Width="100%">
                                    <HeaderTemplate>
                                        Listado</HeaderTemplate>
                                    <ContentTemplate>
                                        <div style="width: 98%; height: 150px;">
                                            <table width="100%">
                                                <tr>
                                                    <td style="text-align: left;" colspan="2">
                                                        <asp:Label ID="Lbl_Busqueda_Listado" runat="server" Text="Búsqueda"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 20%; text-align: left;">
                                                        <asp:Label ID="Lbl_No_Listado" runat="server" Text="No. Listado" CssClass="estilo_fuente"></asp:Label>
                                                    </td>
                                                    <td style="width: 80%; text-align: left;">
                                                        <asp:TextBox ID="Txt_No_Listado" runat="server" Width="200px"></asp:TextBox><cc1:FilteredTextBoxExtender
                                                            ID="FTE_Txt_No_Listado" runat="server" FilterType="Numbers" TargetControlID="Txt_No_Listado"
                                                            Enabled="True">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 20%; text-align: left;">
                                                        <asp:Label ID="Lbl_Fecha_Generacion" runat="server" Text="Fecha Generacion" CssClass="estilo_fuente"></asp:Label>
                                                    </td>
                                                    <td style="width: 80%; text-align: left;">
                                                        <asp:TextBox ID="Txt_Fecha_Generacion" runat="server" Width="200px" Enabled="False"></asp:TextBox><asp:ImageButton
                                                            ID="Btn_Fecha_Generacion" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                                            AlternateText="Seleccione la Fecha de Generacion del Listado" /><cc1:CalendarExtender
                                                                ID="CalendarExtender1" runat="server" TargetControlID="Txt_Fecha_Generacion"
                                                                PopupButtonID="Btn_Fecha_Generacion" Format="dd/MMM/yyyy" Enabled="True">
                                                            </cc1:CalendarExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 20%; text-align: left;">
                                                        <asp:Label ID="Lbl_Notario" runat="server" Text="Notario" CssClass="estilo_fuente"></asp:Label>
                                                    </td>
                                                    <td style="width: 80%; text-align: left;">
                                                        <asp:DropDownList ID="Cmb_Notarios" runat="server" Width="80%">
                                                            <asp:ListItem Text="&lt; SELECCIONE &gt;" Value="SELECCIONE"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        &#160;&#160;&#160;&#160;&#160;&#160;&#160;
                                                        <asp:ImageButton ID="Btn_Buscar_Listado" runat="server" CausesValidation="False"
                                                            ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Buscar Listados"
                                                            OnClick="Btn_Buscar_Listado_Click" /><asp:ImageButton ID="Btn_Limpiar_Filtros_Buscar_Listado"
                                                                runat="server" CausesValidation="False" ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png"
                                                                Width="20px" ToolTip="Limpiar Filtros" OnClick="Btn_Limpiar_Filtros_Buscar_Listado_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                            </cc1:TabContainer><div style="width: 98%">
                                <center>
                                    <caption>
                                        <br />
                                        <asp:GridView ID="Grid_Contrarecibos" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                            CssClass="GridView_1" GridLines="None" Width="98%" OnRowDataBound="Grid_Contrarecibos_RowDataBound"
                                            OnSelectedIndexChanged="Grid_Contrarecibos_SelectedIndexChanged">
                                            <AlternatingRowStyle CssClass="GridAltItem" />
                                            <Columns>
                                                <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                    <ItemStyle Width="30px" />
                                                </asp:ButtonField>
                                                <asp:BoundField DataField="NO_CONTRARECIBO" HeaderText="Contrarecibo" SortExpression="NO_CONTRARECIBO" />
                                                <asp:BoundField DataField="CUENTA_PREDIAL" HeaderText="Cuenta Predial" NullDisplayText="SIN REGISTRO"
                                                    SortExpression="CUENTA_PREDIAL" />
                                                <asp:BoundField DataField="NO_ESCRITURA" HeaderText="No. Escritura" SortExpression="NO_ESCRITURA"
                                                    Visible="False" />
                                                <asp:BoundField DataField="FECHA_ESCRITURA" HeaderText="Fecha Escritura" DataFormatString="{0:dd/MMM/yyyy}"
                                                    SortExpression="FECHA_ESCRITURA" />
                                                <asp:BoundField DataField="FECHA_LIBERACION" HeaderText="Fecha Liberación" DataFormatString="{0:dd/MMM/yyyy}"
                                                    NullDisplayText="//" SortExpression="FECHA_LIBERACION" />
                                                <asp:BoundField DataField="FECHA_PAGO" HeaderText="Fecha Pago" DataFormatString="{0:dd/MMM/yyyy}"
                                                    NullDisplayText="//" SortExpression="FECHA_PAGO" />
                                                <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" SortExpression="ESTATUS"
                                                    Visible="False" />
                                            </Columns>
                                            <HeaderStyle CssClass="GridHeader" />
                                            <PagerStyle CssClass="GridHeader" />
                                            <RowStyle CssClass="GridItem" />
                                            <SelectedRowStyle CssClass="GridSelected" />
                                        </asp:GridView>
                                    </caption>
                                </center>
                            </div>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" HeaderText="Tab_Panel_Calculo_Impuesto" ID="Tab_Panel_Calculo_Impuesto"
                        Width="100%">
                        <HeaderTemplate>
                            Cálculo de Impuesto</HeaderTemplate>
                        <ContentTemplate>
                            <table width="100%" style="width: auto">
                                <tr>
                                    <td style="text-align: left;" colspan="2">
                                        <asp:HiddenField ID="Hidden_Tasa_Trasaldo_Dominio" runat="server" />
                                    </td>
                                    <td class="style2">
                                        &nbsp;
                                    </td>
                                    <td style="width: 20%; text-align: left;">
                                        &#160;
                                    </td>
                                    <td style="width: 20%; text-align: left;">
                                        &nbsp;
                                    </td>
                                    <td style="width: auto; text-align: left;">
                                        &#160;
                                    </td>
                                    <td style="width: auto; text-align: left;">
                                        &#160;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; text-align: left;">
                                        <b>Cuenta Predial</b>
                                    </td>
                                    <td style="width: 20%; text-align: left;">
                                        <asp:Label ID="Lbl_Calculo_Impuesto_Cuenta_Predial" runat="server" Font-Bold="True"
                                            Text="0000000000"></asp:Label>
                                    </td>
                                    <td class="style2">
                                    </td>
                                    <td style="width: 20%; text-align: left;">
                                        <asp:CheckBox ID="Chk_Predio_Colindante" runat="server" AutoPostBack="True" OnCheckedChanged="Chk_Predio_Colindante_CheckedChanged"
                                            Text="Predio Colindante" />
                                    </td>
                                    <td style="width: 20%; text-align: left;">
                                    </td>
                                    <td style="width: auto; text-align: left;">
                                    </td>
                                    <td style="width: auto; text-align: left;">
                                        &#160;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; text-align: left;">
                                        *Base del Impuesto
                                    </td>
                                    <td style="width: 20%; text-align: left;">
                                        <asp:TextBox ID="Txt_Calculo_Impuesto_Base_Impuesto" runat="server" AutoPostBack="True"
                                            MaxLength="12" OnTextChanged="Txt_Calculo_Impuesto_Base_Impuesto_TextChanged">0.00</asp:TextBox><cc1:FilteredTextBoxExtender
                                                ID="FTE_Txt_Calculo_Impuesto_Base_Impuesto" runat="server" Enabled="True" TargetControlID="Txt_Calculo_Impuesto_Base_Impuesto"
                                                ValidChars="$0123456789.">
                                            </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td class="style2">
                                        <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Names="Comic Sans MS"
                                            Text="-"></asp:Label>
                                    </td>
                                    <td style="width: 20%; text-align: left;">
                                        Mínimo elevado al año
                                    </td>
                                    <td style="width: 20%; text-align: left;">
                                        <asp:TextBox ID="Txt_Calculo_Impuesto_Minimo_Elevado_Año" runat="server" AutoPostBack="True"
                                            OnTextChanged="Txt_Calculo_Impuesto_Minimo_Elevado_Año_TextChanged" MaxLength="12">0.00</asp:TextBox><cc1:FilteredTextBoxExtender
                                                ID="FTE_Txt_Calculo_Impuesto_Minimo_Elevado_Año" runat="server" Enabled="True"
                                                TargetControlID="Txt_Calculo_Impuesto_Minimo_Elevado_Año" ValidChars="$0123456789.">
                                            </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td style="width: auto; text-align: left;">
                                        <asp:ImageButton ID="Btn_Tasas_Traslado_Dominio" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                            OnClick="Btn_Tasas_Traslado_Dominio_Click" ToolTip="Tasas" />
                                    </td>
                                    <td style="width: auto; text-align: left;">
                                        &#160;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; text-align: left;">
                                        Base Gravable T. de Dominio
                                    </td>
                                    <td style="width: 20%; text-align: left;">
                                        <asp:Label ID="Lbl_Calculo_Impuesto_Base_Gravable_T_Dominio" runat="server" Text="0.00"></asp:Label>
                                    </td>
                                    <td class="style2">
                                        <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Names="Comic Sans MS"
                                            Text="x"></asp:Label>
                                    </td>
                                    <td style="width: 20%; text-align: left;">
                                        Tasa Traslado de Dominio
                                    </td>
                                    <td style="width: 20%; text-align: left;">
                                        <asp:TextBox ID="Txt_Calculo_Impuesto_Tasa_Traslado_Dominio" runat="server" AutoPostBack="True"
                                            MaxLength="12" OnTextChanged="Txt_Calculo_Impuesto_Tasa_Traslado_Dominio_TextChanged">0.00</asp:TextBox><cc1:FilteredTextBoxExtender
                                                ID="FTE_Txt_Calculo_Impuesto_Tasa_Traslado_Dominio" runat="server" Enabled="True"
                                                TargetControlID="Txt_Calculo_Impuesto_Tasa_Traslado_Dominio" ValidChars="0123456789.%">
                                            </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td style="width: auto; text-align: left;">
                                        &nbsp;
                                    </td>
                                    <td style="width: auto; text-align: left;">
                                        &#160;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; text-align: left;">
                                        &#160;
                                    </td>
                                    <td style="width: 20%; text-align: left;">
                                        &#160;
                                    </td>
                                    <td class="style2">
                                        &#160;
                                    </td>
                                    <td style="width: 20%; text-align: right;">
                                        <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Names="Comic Sans MS"
                                            Text="="></asp:Label>
                                    </td>
                                    <td style="width: 20%; text-align: left;">
                                        Impuesto Trasladado Dominio
                                    </td>
                                    <td style="width: auto; text-align: left;">
                                        <asp:Label ID="Lbl_Calculo_Impuesto_Impuesto_Traslado_Dominio" runat="server" Text="0.00"></asp:Label>
                                    </td>
                                    <td style="width: auto; text-align: left;">
                                        &#160;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; text-align: left;">
                                        &#160;
                                    </td>
                                    <td style="width: 20%; text-align: left;">
                                        &nbsp;
                                    </td>
                                    <td class="style2">
                                        &nbsp;
                                    </td>
                                    <td style="width: 20%; text-align: left;">
                                        &#160;
                                    </td>
                                    <td style="width: 20%; text-align: left;">
                                        &#160;
                                    </td>
                                    <td style="width: auto; text-align: left;">
                                        &#160;
                                    </td>
                                    <td style="width: auto; text-align: left;">
                                        &#160;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; text-align: left;">
                                        Tipo División o Lotificación
                                    </td>
                                    <td style="width: 20%; text-align: left;">
                                        <asp:Label ID="Lbl_Calculo_Impuesto_Tipo_Division_Lotificacion" runat="server" Text="Seleccione uno --&gt;"></asp:Label>
                                    </td>
                                    <td class="style2">
                                        <asp:ImageButton ID="Btn_Tipo_Division_Lotificacion" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                            OnClick="Btn_Tipo_Division_Lotificacion_Click" TabIndex="8" ToolTip="Divisiones y Lotificaciones" />
                                    </td>
                                    <td style="width: 20%; text-align: left;">
                                        &nbsp;
                                    </td>
                                    <td style="width: 20%; text-align: left;">
                                        &#160;
                                    </td>
                                    <td style="width: auto; text-align: left;">
                                        &#160;
                                    </td>
                                    <td style="width: auto; text-align: left;">
                                        &#160;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; text-align: left;">
                                        *Base del Impuesto
                                    </td>
                                    <td style="width: 20%; text-align: left;">
                                        <asp:TextBox ID="Txt_Calculo_Impuesto_Base_Impuesto_2" runat="server" AutoPostBack="True"
                                            MaxLength="10" OnTextChanged="Txt_Calculo_Impuesto_Base_Impuesto_2_TextChanged">0.00</asp:TextBox><cc1:FilteredTextBoxExtender
                                                ID="FTE_Txt_Calculo_Impuesto_Base_Impuesto_2" runat="server" Enabled="True" TargetControlID="Txt_Calculo_Impuesto_Base_Impuesto_2"
                                                ValidChars="$0123456789.">
                                            </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td class="style2">
                                        <asp:Label ID="Label19" runat="server" Font-Bold="True" Font-Names="Comic Sans MS"
                                            Text="x"></asp:Label>
                                    </td>
                                    <td style="width: 20%; text-align: left;">
                                        Tasa División o Lotificación
                                    </td>
                                    <td style="width: 20%; text-align: left;">
                                        <asp:Label ID="Lbl_Calculo_Impuesto_Tasa_Division_Lotificacion" runat="server" Text="0.00"></asp:Label>
                                    </td>
                                    <td style="width: auto; text-align: left;">
                                        &nbsp;
                                    </td>
                                    <td style="width: auto; text-align: left;">
                                        &#160;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; text-align: left;">
                                        &#160;
                                    </td>
                                    <td style="width: 20%; text-align: left;">
                                        &#160;
                                    </td>
                                    <td class="style2">
                                        &#160;
                                    </td>
                                    <td style="width: 20%;" class="style1">
                                        <asp:Label ID="Label20" runat="server" Font-Bold="True" Font-Names="Comic Sans MS"
                                            Text="="></asp:Label>
                                    </td>
                                    <td style="width: 20%; text-align: left;">
                                        Imp. Div./Lot.
                                    </td>
                                    <td style="width: auto; text-align: left;">
                                        <asp:Label ID="Lbl_Calculo_Impuesto_Imp_Div_Lot" runat="server" Text="0.00"></asp:Label>
                                    </td>
                                    <td style="width: auto; text-align: left;">
                                        &#160;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; text-align: left;">
                                        &#160;
                                    </td>
                                    <td style="width: 20%; text-align: left;">
                                        &#160;
                                    </td>
                                    <td class="style2">
                                        &#160;
                                    </td>
                                    <td style="width: 20%;" class="style1">
                                        <asp:Label ID="Label21" runat="server" Font-Bold="True" Font-Names="Comic Sans MS"
                                            Text="+"></asp:Label>
                                    </td>
                                    <td style="width: 20%; text-align: left;">
                                        Constancia No Adeudo
                                    </td>
                                    <td style="width: auto; text-align: left;">
                                        <asp:Label ID="Lbl_Calculo_Impuesto_Constancia_No_Adeudo" runat="server" Text="0.00"></asp:Label>
                                    </td>
                                    <td style="width: auto; text-align: left;">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; text-align: left;">
                                        &#160;
                                    </td>
                                    <td style="width: 20%; text-align: left;">
                                        &#160;
                                    </td>
                                    <td class="style2">
                                        &#160;
                                    </td>
                                    <td style="width: 20%;" class="style1">
                                        <asp:Label ID="Label22" runat="server" Font-Bold="True" Font-Names="Comic Sans MS"
                                            Text="+"></asp:Label>
                                    </td>
                                    <td style="width: 20%; text-align: left;">
                                        Multa
                                    </td>
                                    <td style="width: auto; text-align: left;">
                                        <asp:Label ID="Lbl_Calculo_Impuesto_Multa" runat="server" Text="0.00"></asp:Label>
                                    </td>
                                    <td style="width: auto; text-align: left;">
                                        <asp:ImageButton ID="Btn_Multas" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                            OnClick="Btn_Multas_Click" TabIndex="8" ToolTip="Multas" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; text-align: left;">
                                        &#160;
                                    </td>
                                    <td style="width: 20%; text-align: left;">
                                        &#160;
                                    </td>
                                    <td class="style2">
                                        &#160;
                                    </td>
                                    <td style="width: 20%;" class="style1">
                                        <asp:Label ID="Label23" runat="server" Font-Bold="True" Font-Names="Comic Sans MS"
                                            Text="+"></asp:Label>
                                    </td>
                                    <td style="width: 20%; text-align: left;">
                                        Recargos
                                    </td>
                                    <td style="width: auto; text-align: left;">
                                        <asp:TextBox ID="Txt_Calculo_Impuesto_Recargos" runat="server" AutoPostBack="True"
                                            MaxLength="12" OnTextChanged="Txt_Calculo_Impuesto_Tasa_Traslado_Dominio_TextChanged">0.00</asp:TextBox><cc1:FilteredTextBoxExtender
                                                ID="FTE_Txt_Calculo_Impuesto_Recargos_FilteredTextBoxExtender" runat="server"
                                                Enabled="True" TargetControlID="Txt_Calculo_Impuesto_Recargos" ValidChars="$0123456789.">
                                            </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td style="width: auto; text-align: left;">
                                        &#160;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; text-align: left;">
                                        &#160;
                                    </td>
                                    <td style="width: 20%; text-align: left;">
                                        &#160;
                                    </td>
                                    <td class="style2">
                                        &#160;
                                    </td>
                                    <td class="style1" style="width: 20%;">
                                        <asp:Label ID="Label24" runat="server" Font-Bold="True" Font-Names="Comic Sans MS"
                                            Text="="></asp:Label>
                                    </td>
                                    <td style="width: 20%; text-align: left;">
                                        Total
                                    </td>
                                    <td style="width: auto; text-align: left;">
                                        <asp:Label ID="Lbl_Calculo_Impuesto_Total" runat="server" Text="0.00"></asp:Label>
                                    </td>
                                    <td style="width: auto; text-align: left;">
                                        <asp:ImageButton ID="Btn_Calcular" runat="server" ImageUrl="~/paginas/imagenes/paginas/SIAS_Calc3.gif"
                                            TabIndex="8" ToolTip="Calcular" OnClick="Btn_Calcular_Click" />
                                    </td>
                                </tr>
                            </table>
                            <div class="style4">
                                <br />
                                &nbsp;&nbsp;</div>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
                <br />
                <asp:Panel ID="Panel1" runat="server">
                </asp:Panel>
                <br />
            </div>
            <br />
            <br />
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

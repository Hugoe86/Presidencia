<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Caj_Pagos.aspx.cs" Inherits="paginas_Predial_Frm_Ope_Caj_Pagos"
    Title="Pantalla de Cobro" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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

    <script type="text/javascript" language="javascript">

    //Metodos para abrir los Modal PopUp's de la página
    function Abrir_Busqueda_Cuentas_Predial() {
        $find('Busqueda_Cuentas_Predial').show();
        return false;
    }
    function Abrir_Detalle_Cuenta_Predial() {
        $find('Detalle_Cuenta_Predial').show();
        return false;
    }

    </script>

    <asp:ScriptManager ID="ScriptManager_Caja_Pago" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Caja_Pago" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Caja_Pago" style="background-color: #ffffff; width: 98%; height: 100%;">
                <table width="100%" border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo">
                            Pago en Caja
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png"
                                Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table width="98%" border="0" cellspacing="0">
                    <tr align="center">
                        <td>
                            <div align="right" class="barra_busqueda">
                                <table style="width: 100%; height: 28px;">
                                    <tr>
                                        <td align="left">
                                            <asp:UpdatePanel ID="Upnl_Botones_Operacion" runat="server" UpdateMode="Conditional"
                                                RenderMode="Inline">
                                                <ContentTemplate>
                                                    <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button"
                                                        TabIndex="1" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" OnClick="Btn_Nuevo_Click" />
                                                    <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender_Btn_Nuevo" runat="server" ConfirmText="¿Esta seguro de enviar a impresión el recibo?"
                                                        TargetControlID="Btn_Nuevo">
                                                    </cc1:ConfirmButtonExtender>
                                                    <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                                        TabIndex="3" CausesValidation="false" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                                        OnClick="Btn_Salir_Click" />
                                                    <asp:TextBox ID="Txt_Caja_ID" runat="server" Visible="false" ReadOnly="true"></asp:TextBox>
                                                    <asp:TextBox ID="Txt_No_Turno" runat="server" Visible="false" ReadOnly="true"></asp:TextBox>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
                <asp:UpdatePanel ID="Upnl_Datos_Generales_Ingresos" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="Pnl_Datos_Generales" runat="server" GroupingText="Datos Generales Ingreso"
                            Width="97%" BackColor="White">
                            <table style="width: 100%;">
                                <tr>
                                    <td style="width: 20%">
                                        Folio
                                    </td>
                                    <td style="width: 30%">
                                        <asp:TextBox ID="Txt_Busqueda_Referencia" runat="server" MaxLength="100" TabIndex="4"
                                            ToolTip="Burcar por Folio" Width="170px" OnTextChanged="Txt_Busqueda_Referencia_TextChanged" />
                                        <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Referencia" runat="server" WatermarkCssClass="watermarked"
                                            WatermarkText="<Ingrese el Folio>" TargetControlID="Txt_Busqueda_Referencia" />
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Referencia" runat="server" TargetControlID="Txt_Busqueda_Referencia"
                                            FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ.- ">
                                        </cc1:FilteredTextBoxExtender>
                                        <asp:ImageButton ID="Btn_Buscar_Referencia" runat="server" ToolTip="Consultar" TabIndex="5"
                                            ImageUrl="~/paginas/imagenes/paginas/busqueda.png" OnClick="Btn_Buscar_Referencia_Click" />
                                    </td>
                                    <td style="width: 20%; text-align: left;">
                                        Estatus
                                    </td>
                                    <td style="width: 30%; text-align: left;">
                                        <asp:TextBox ID="Txt_Estatus_Ingresos" runat="server" ReadOnly="true" Width="98%"
                                            BorderStyle="Solid" BorderWidth="1"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <div id="Div_Datos_Cuenta_Predial_Pasivo" runat="server" visible="false">
                                <table width="100%" border="0" cellspacing="0" class="estilo_fuente">
                                    <tr align="center">
                                        <td>
                                            <asp:GridView ID="Grid_Folios_Cuenta_Predial" runat="server" AllowPaging="True" Width="100%"
                                                AutoGenerateColumns="False" CssClass="GridView_1" OnPageIndexChanging="Grid_Folios_Cuenta_Predial_PageIndexChanging"
                                                GridLines="None" HeaderStyle-CssClass="tblHead" OnSelectedIndexChanged="Grid_Folios_Cuenta_Predial_SelectedIndexChanged">
                                                <Columns>
                                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                        <ItemStyle Width="30px" />
                                                    </asp:ButtonField>
                                                    <asp:BoundField DataField="Referencia" HeaderText="Referencia" Visible="True">
                                                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Descripcion" HeaderText="Descripción" Visible="True">
                                                        <HeaderStyle HorizontalAlign="Left" Width="70%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="70%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Estatus" HeaderText="Estatus" Visible="True">
                                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <SelectedRowStyle CssClass="GridSelected" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <table width="100%" border="0" cellspacing="0" class="estilo_fuente">
                                <tr align="center">
                                    <td>
                                        <asp:GridView ID="Grid_Folios_Ingresos_Pasivos" runat="server" AllowPaging="True"
                                            Width="100%" AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None"
                                            HeaderStyle-CssClass="tblHead">
                                            <Columns>
                                                <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" Visible="True">
                                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Ingreso" HeaderText="Ingreso" Visible="True">
                                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Fecha_Ingreso" HeaderText="Fecha" Visible="True" DataFormatString="{0:dd/MMM/yyyy}">
                                                    <HeaderStyle HorizontalAlign="Left" Width="12%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="12%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Fecha_Vencimiento" HeaderText="Vencimiento" Visible="True"
                                                    DataFormatString="{0:dd/MMM/yyyy}">
                                                    <HeaderStyle HorizontalAlign="Left" Width="12%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="12%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Monto" HeaderText="Monto" Visible="True" DataFormatString="{0:#,##0.00}">
                                                    <HeaderStyle HorizontalAlign="Center" Width="12%" />
                                                    <ItemStyle HorizontalAlign="Right" Width="12%" CssClass="text_cantidades_grid" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Recargos" HeaderText="Recargos" Visible="True" DataFormatString="{0:#,##0.00}">
                                                    <HeaderStyle HorizontalAlign="Center" Width="12%" />
                                                    <ItemStyle HorizontalAlign="Right" Width="12%" CssClass="text_cantidades_grid" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Total_Pagar" HeaderText="Total" Visible="True" DataFormatString="{0:#,##0.00}">
                                                    <HeaderStyle HorizontalAlign="Center" Width="12%" />
                                                    <ItemStyle HorizontalAlign="Right" Width="12%" CssClass="text_cantidades_grid" />
                                                </asp:BoundField>
                                            </Columns>
                                            <SelectedRowStyle CssClass="GridSelected" />
                                            <PagerStyle CssClass="GridHeader" />
                                            <AlternatingRowStyle CssClass="GridAltItem" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                            <table style="width: 100%;">
                                <%--<tr>
                                    <td style="width:20%;text-align:left;">Descripcion</td>
                                    <td colspan="3" style="width:80%;text-align:left;">
                                        <asp:TextBox ID="Txt_Descripcion_Ingresos" runat="server" ReadOnly="true" Width="99%" BorderStyle="Solid" BorderWidth="1"></asp:TextBox> 
                                    </td>
                                </tr> --%>
                                <tr>
                                    <%--<td style="width:20%;text-align:left;">Ingresos</td>
                                    <td style="width:30%;text-align:left;">
                                        <asp:TextBox ID="Txt_Ingresos" runat="server" ReadOnly="true" Width="99%" BorderStyle="Solid" BorderWidth="1"></asp:TextBox>
                                    </td>--%>
                                    <td colspan="2" width="50%">
                                    </td>
                                    <td style="width: 20%; text-align: left;">
                                        Importe
                                    </td>
                                    <td style="width: 30%; text-align: left;">
                                        <asp:TextBox ID="Txt_Importe_Ingresos" runat="server" ReadOnly="true" Width="98%"
                                            BorderStyle="Solid" BorderWidth="1" CssClass="text_cantidades_grid"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <%--<td style="width:20%;text-align:left;">Fecha</td>
                                    <td style="width:30%;text-align:left;">
                                        <asp:TextBox ID="Txt_Fecha_Tramite_Ingresos" runat="server" ReadOnly="true" Width="98%" BorderStyle="Solid" BorderWidth="1"></asp:TextBox>
                                    </td>--%>
                                    <td colspan="2" width="50%">
                                    </td>
                                    <td style="width: 20%; text-align: left;">
                                        Ajuste Tarifario
                                    </td>
                                    <td style="width: 30%; text-align: left;">
                                        <asp:TextBox ID="Txt_Ajuste_Tarifario" runat="server" ReadOnly="true" Width="98%"
                                            BorderStyle="Solid" BorderWidth="1" CssClass="text_cantidades_grid"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <%--<td style="width:20%;text-align:left;">Vencimiento</td>
                                    <td style="width:30%;text-align:left;">
                                        <asp:TextBox ID="Txt_Fecha_Vencimiento" runat="server" ReadOnly="true" Width="98%" BorderStyle="Solid" BorderWidth="1"></asp:TextBox>
                                    </td>--%>
                                    <td colspan="2" width="50%">
                                    </td>
                                    <td style="width: 20%; text-align: left;">
                                        Total Pagar
                                    </td>
                                    <td style="width: 30%; text-align: left;">
                                        <asp:TextBox ID="Txt_Total_Pagar_Ingreso" runat="server" ReadOnly="true" Width="98%"
                                            BorderStyle="Solid" BorderWidth="1" CssClass="text_cantidades_grid" ForeColor="Red"
                                            Font-Bold="true" Height="25px" Font-Size="Large"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="Upnl_Datos_Generales_Pagos_Ingresos" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="Pnl_Datos_Generales_Pagos" runat="server" GroupingText="Datos Generales Pago"
                            Width="97%" BackColor="White">
                            <table style="width: 100%;">
                                <tr class="estilo_fuente">
                                    <td style="width: 20%; text-align: left; font-size: medium;">
                                        No. Recibo
                                    </td>
                                    <td style="width: 30%; text-align: right;">
                                        <asp:TextBox ID="Txt_No_Recibo_Pago" runat="server" ReadOnly="true" Width="98%" CssClass="text_cantidades_grid"
                                            BorderStyle="Solid" BorderWidth="1" Font-Size="X-Large" Font-Bold="true" ForeColor="Red"
                                            Height="30px"></asp:TextBox>
                                    </td>
                                    <td style="width: 20%; text-align: left; font-size: medium;">
                                        Fecha Aplicación
                                    </td>
                                    <td style="width: 30%; text-align: right;">
                                        <asp:TextBox ID="Txt_Fecha_Aplicacion" runat="server" ReadOnly="true" Width="97%"
                                            CssClass="text_cantidades_grid" BorderStyle="Solid" BorderWidth="1" Font-Size="X-Large"
                                            Font-Bold="true" Height="30px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <table style="width: 100%;">
                                <tr style="background-color: #36C;">
                                    <td style="text-align: left; font-size: 15px; color: #FFF;" colspan="6">
                                        Forma de pago
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; width: 30%;" colspan="2">
                                        <asp:ImageButton ID="Btn_Pago_Efectivo" runat="server" CssClass="Img_Button" TabIndex="1"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_efectivo.jpg" Width="35px" Height="35px"
                                            OnClick="Btn_Pago_Efectivo_Click" ToolTip="en Efectivo" />
                                        &nbsp;<asp:ImageButton ID="Btn_Pago_Banco" runat="server" CssClass="Img_Button" TabIndex="1"
                                            ImageUrl="~/paginas/imagenes/paginas/tarjetas-credito-debito.png" Width="35px"
                                            Height="35px" OnClick="Btn_Pago_Banco_Click" ToolTip="con Tarjeta Bancaria" />
                                        &nbsp;<asp:ImageButton ID="Btn_Pago_Cheque" runat="server" CssClass="Img_Button"
                                            TabIndex="1" ImageUrl="~/paginas/imagenes/paginas/icono_cheque.png" Width="35px"
                                            Height="35px" OnClick="Btn_Pago_Cheque_Click" ToolTip="con Cheque" />
                                        &nbsp;<asp:ImageButton ID="Btn_Pago_Transferencia" runat="server" CssClass="Img_Button"
                                            TabIndex="1" ImageUrl="~/paginas/imagenes/paginas/wire_transfer_48.png" Width="35px"
                                            Height="35px" OnClick="Btn_Pago_Transferencia_Click" ToolTip="por Transferencia" />
                                    </td>
                                    <td style="text-align: left; width: 15%; font-size: medium;">
                                        Total Pagado
                                    </td>
                                    <td style="text-align: right; width: 20%;">
                                        <asp:TextBox ID="Txt_Total_Pagado" runat="server" ReadOnly="true" Width="98%" CssClass="text_cantidades_grid"
                                            BorderStyle="Solid" BorderWidth="1" Font-Size="X-Large" Font-Bold="true" ForeColor="Red"
                                            Height="30px" />
                                    </td>
                                    <td style="text-align: left; width: 15%; font-size: medium;">
                                        <asp:Label ID="Lbl_Cambio" runat="server" Text="Cambio" Style="text-align: left;
                                            width: 15%; font-size: medium;"></asp:Label>
                                    </td>
                                    <td style="text-align: right; width: 20%;">
                                        <asp:TextBox ID="Txt_Cambio_Ingresos" runat="server" ReadOnly="true" Width="98%"
                                            CssClass="text_cantidades_grid" BorderStyle="Solid" BorderWidth="1" Font-Size="X-Large"
                                            Font-Bold="true" ForeColor="Red" Height="30px"></asp:TextBox>
                                    </td>
                                </tr>
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="text-align: left; width: 20%; vertical-align: middle;" rowspan="2">
                                            <asp:Label ID="Lbl_Forma_Pago" runat="server" Text="EFECTIVO" Font-Bold="True" Font-Size="Medium"
                                                ForeColor="#FF6600"></asp:Label>
                                        </td>
                                        <td style="text-align: left; width: 20%;">
                                            <asp:Label ID="Lbl_Forma_Pago_Banco" runat="server" Text="Banco"></asp:Label>
                                        </td>
                                        <td style="text-align: left; width: 15%;">
                                            <asp:Label ID="Lbl_Forma_Pago_Referencia" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td style="text-align: left; width: 20%;">
                                            <asp:Label ID="Lbl_Forma_Pago_Plan_Pago" runat="server" Text="Plan de Pago"></asp:Label>
                                        </td>
                                        <td style="text-align: right; width: 15%;">
                                            <asp:Label ID="Lbl_Forma_Pago_Monto" runat="server" Text="Monto"></asp:Label>
                                        </td>
                                        <td style="text-align: right; width: 5%;">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right; width: 20%;">
                                            <asp:DropDownList ID="Cmb_Forma_Pago_Bancos" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Cmb_Bancos_SelectedIndexChanged"
                                                TabIndex="7" Width="99%" Height="25px" Font-Bold="false" Font-Size="Medium" />
                                        </td>
                                        <td style="text-align: right; width: 15%;">
                                            <asp:TextBox ID="Txt_Forma_Pago_Banco_Referencia" runat="server" TabIndex="8" Width="60%"
                                                Height="25px" Font-Bold="false" Font-Size="Medium"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers"
                                                TargetControlID="Txt_Forma_Pago_Banco_Referencia" ValidChars="ÑñáéíóúÁÉÍÓÚ. ">
                                            </cc1:FilteredTextBoxExtender>
                                            <asp:TextBox ID="Txt_Forma_Pago_Banco_No_Tarjeta" runat="server" TabIndex="8" Width="30%"
                                                Height="25px" Font-Bold="false" Font-Size="Medium" MaxLength="4"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Numbers"
                                                TargetControlID="Txt_Forma_Pago_Banco_No_Tarjeta">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                        <td style="text-align: right; width: 15%;">
                                            <asp:DropDownList ID="Cmb_Plan_Pago_Bancario" runat="server" TabIndex="9" Width="99%"
                                                Height="25px" Font-Bold="false" Font-Size="Large">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="text-align: right; width: 15%;">
                                            <asp:TextBox ID="Txt_Forma_Pago_Monto" runat="server" CssClass="text_cantidades_grid"
                                                Font-Bold="true" Font-Size="Medium" ForeColor="Blue" Height="25px" TabIndex="11"
                                                Width="97%" Visible="True" AutoPostBack="True" OnTextChanged="Txt_Forma_Pago_Monto_TextChanged"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Forma_Pago_Monto" runat="server" FilterType="Custom, Numbers"
                                                TargetControlID="Txt_Forma_Pago_Monto" ValidChars="." />
                                        </td>
                                        <td style="text-align: right; width: 5%;">
                                            <asp:ImageButton ID="Img_Btn_Agregar_Forma_Pago" runat="server" Height="24px" ImageUrl="../imagenes/paginas/sias_add.png"
                                                OnClick="Img_Btn_Agregar_Forma_Pago_Click" Style="vertical-align: middle;" TabIndex="10"
                                                ToolTip="Agregar Forma de Pago" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6" style="text-align: center; width: 100%;">
                                            <asp:GridView ID="Grid_Formas_Pago" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                CssClass="GridView_1" GridLines="None" HeaderStyle-CssClass="tblHead" Width="100%"
                                                OnRowCommand="Grid_Formas_Pago_RowDataCommand">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="Img_Btn_Eliminar" runat="server" CausesValidation="False" CommandName="Eliminar"
                                                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ImageUrl="~/paginas/imagenes/gridview/grid_garbage.png" />
                                                            <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender_Img_Btn_Eliminar" TargetControlID="Img_Btn_Eliminar"
                                                                ConfirmText="¿Esta seguro de elimnar la forma de pago?" runat="server">
                                                            </cc1:ConfirmButtonExtender>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="3%" />
                                                        <ItemStyle Width="3%" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Forma_Pago" HeaderText="Forma Pago" Visible="True">
                                                        <HeaderStyle Width="20%" />
                                                        <ItemStyle Width="20%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Banco" HeaderText="Banco" Visible="True">
                                                        <HeaderStyle Width="25%" />
                                                        <ItemStyle Width="25%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="Referencia" Visible="True" DataField="Referencia">
                                                        <HeaderStyle Width="25%" />
                                                        <ItemStyle Width="25%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Monto" HeaderText="Monto" DataFormatString="{0:c}">
                                                        <HeaderStyle HorizontalAlign="Right" Width="20%" />
                                                        <ItemStyle HorizontalAlign="Right" Width="20%" />
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
                                <asp:HiddenField ID="Hdn_Tasa_ID" runat="server" />
                                <asp:HiddenField ID="Hdn_Propietario_ID" runat="server" />
                                <asp:HiddenField ID="Hdn_Cuenta_Predial_ID" runat="server" />
                                <asp:HiddenField ID="Hdn_Cuenta_Predial" runat="server" />
                                <asp:HiddenField ID="Hdn_No_Caja" runat="server" />
                                <asp:HiddenField ID="Hdn_No_Pago" runat="server" />
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

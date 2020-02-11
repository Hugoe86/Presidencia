<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Pre_Historial_Pagos.aspx.cs" Inherits="paginas_Predial_Frm_Ope_Pre_Historial_Pagos" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<style type="text/css" >
.TooltipAdeudos 
{
    width:240px;
    padding:30px 30px 25px 18px;
    text-align:left;
    border:none;
    line-height:30px;
    background: transparent url(../imagenes/TooltipAdeudos500.png) no-repeat 0 0;
}
.Tabla_Comentarios 
{
    border-collapse:collapse;
    margin-left:25px;
    color: #25406D;
    font-family: Verdana,Geneva,MS Sans Serif;
    font-size: small;
    text-align: left;
    font-size:11px;
    line-height:15px;
}
.Tabla_Comentarios, .Tabla_Comentarios th, .Tabla_Comentarios td
{
    border: 1px solid #999999;
    padding: 0;
    font-size:11px;
}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
 <script type="text/javascript" language="javascript">
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
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
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
    <asp:ScriptManager ID="ScriptManager" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
        
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <%--<ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>--%>
            </asp:UpdateProgress>
            
            <div id="Div_Contenido" style="background-color:#ffffff; width:100%; height:100%;">
                    
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="4" class="label_titulo">
                            Historial de pagos
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">&nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
               </table>             
            
               <table width="98%"  border="0" cellspacing="0">
                <tr align="center">
                    <td colspan="2">                
                        <div align="right" style="width:99%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold; font-style: normal; font-variant: normal; font-family: fantasy; height:32px"  >
                            <table style="width:100%;height:28px;">
                                <tr>
                                    <td align="left" style="width:59%;">
                                        <asp:ImageButton ID="Btn_Imprimir" runat="server" ToolTip="Imprimir" 
                                            CssClass="Img_Button" TabIndex="4"
                                            ImageUrl="~/paginas/imagenes/gridview/grid_print.png" 
                                            onclick="Btn_Imprimir_Click" />
                                        <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" TabIndex="5"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" />
                                    </td>
                                    <td align="right" style="width:41%;">
                                        <table style="width:100%;height:28px;">
                                            <tr>
                                                <td style="vertical-align:middle;text-align:right;width:20%;"></td>
                                                <td style="width:55%;">
                                                    
                                                    
                                                </td>
                                                <td style="vertical-align:middle;width:5%;" >
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
                    
                <table width="98%" class="estilo_fuente">
                    <tr>
                        <td style="text-align:left;width:20%;">
                            Cuenta Predial
                        </td>
                        <td style="width:82%; text-align:left;" colspan="3" >
                            <asp:TextBox ID="Txt_Cuenta_Predial" runat="server" Width="35.7%" AutoPostBack="true"
                                 TabIndex="9" MaxLength="20" ></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Cuenta_Predial" runat="server" TargetControlID="Txt_Cuenta_Predial" 
                                FilterType="UppercaseLetters, LowercaseLetters, Numbers"/>
                            &nbsp;
                            B&uacute;squeda: 
                                     <asp:ImageButton ID="Btn_Mostrar_Busqueda_Avanzada" runat="server" ToolTip="Búsqueda Avanzada de Cuenta Predial" 
                                        TabIndex="10" 
                                ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="24px" 
                                Width="24px" onclick="Btn_Mostrar_Busqueda_Avanzada_Click"/>
                            Detalles cuenta:
                                <asp:ImageButton ID="Btn_Detalles_Cuenta_Predial" runat="server" ToolTip="Detalles de la cuenta" 
                                    TabIndex="10" 
                                ImageUrl="~/paginas/imagenes/paginas/icono_consultar.png" Height="24px" 
                                Width="24px" onclick="Btn_Detalles_Cuenta_Predial_Click" />
                        </td> 
                    </tr>
                    
                    <tr>
                        <td style="text-align:left;width:20%;">
                            Entre fecha
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Fecha_Inicial" runat="server" Width="84%" TabIndex="12" AutoPostBack ="true"
                                MaxLength="11" Height="18px" ontextchanged="Txt_Fecha_Inicial_TextChanged" />
                            <cc1:TextBoxWatermarkExtender ID="TBWE_Txt_Fecha_Inicial" runat="server" 
                                TargetControlID="Txt_Fecha_Inicial" WatermarkCssClass="watermarked" 
                                WatermarkText="Dia/Mes/Año" Enabled="True" />
                            <cc1:CalendarExtender ID="CE_Txt_Fecha_Inicial" runat="server" 
                                TargetControlID="Txt_Fecha_Inicial"  Format="dd/MM/yyyy"  Enabled="True" PopupButtonID="Btn_Txt_Fecha_Inicial"/>
                             <asp:ImageButton ID="Btn_Txt_Fecha_Inicial" runat="server"
                                ImageUrl="../imagenes/paginas/SmallCalendar.gif" style="vertical-align:top;"
                                Height="18px" CausesValidation="false"/>
                        </td>
                        <td style="text-align:left;width:20%;text-align:right;">
                            Y fecha
                        </td>
                        <td style="width:31.5%;text-align:left;">
                                <asp:TextBox ID="Txt_Fecha_Final" runat="server" Width="84%" TabIndex="12" AutoPostBack ="true" 
                                    MaxLength="11" Height="18px" ontextchanged="Txt_Fecha_Final_TextChanged" />
                                <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Fecha_Final" runat="server" 
                                    TargetControlID="Txt_Fecha_Final" WatermarkCssClass="watermarked" 
                                    WatermarkText="Dia/Mes/Año" Enabled="True" />
                                <cc1:CalendarExtender ID="CE_Txt_Fecha_Final" runat="server" 
                                    TargetControlID="Txt_Fecha_Final"  Format="dd/MM/yyyy"  Enabled="True" PopupButtonID="Btn_Txt_Fecha_Final"/>
                                 <asp:ImageButton ID="Btn_Txt_Fecha_Final" runat="server"
                                    ImageUrl="../imagenes/paginas/SmallCalendar.gif" style="vertical-align:top;"
                                    Height="18px" CausesValidation="false"/>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            Recibo inicial
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Recibo_Inicial" runat="server" AutoPostBack ="true"
                                Width="96.4%" ontextchanged="Txt_Recibo_Inicial_TextChanged"/>
                        </td>
                        <td style="text-align:left;width:20%;text-align:right;">
                            Recibo final
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Recibo_Final" runat="server" Width="96.4%" AutoPostBack ="true"
                                ontextchanged="Txt_Recibo_Final_TextChanged"/>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            Lugar de pago
                             
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Lugar_Pago" runat="server" Width="99%" TabIndex="7" 
                                onselectedindexchanged="Cmb_Lugar_Pago_SelectedIndexChanged">
                                <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE" />
                            </asp:DropDownList>
                        </td>
                        <td style="text-align:left;width:20%;text-align:right;">
                            Caja
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Caja" runat="server" Width="96.4%"/>
                        </td>
                    </tr>
                    
                    <tr align="center">
                        <td colspan="4">
                            <br />
                            <asp:GridView ID="Grid_Pagos" runat="server" AllowPaging="true" GridLines ="None"  
                                AutoGenerateColumns="True" CssClass="GridView_1" HeaderStyle-CssClass="tblHead" 
                                onselectedindexchanged="Grid_Pagos_SelectedIndexChanged" PageSize="5"
                                EmptyDataText="&quot;No se encontraron registros&quot;"
                                OnPageIndexChanging = "Grid_Pagos_PageIndexChanging" 
                                style="white-space:normal;" Width="100%" >
                                <RowStyle CssClass ="GridItem" />
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="FOLIO" HeaderText="Folio" Visible="True" >
                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CUENTA_PREDIAL_ID" Visible="False" >
                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NO_RECIBO" HeaderText="No. recibo" Visible="True" >
                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NO_OPERACION" HeaderText="No. Ope" Visible="True" >
                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NUMERO_CAJA" HeaderText="No. Caja" Visible="True" >
                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CLAVE_BANCO" HeaderText="Clave Banco" Visible="True" >
                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                    </asp:BoundField>
                                  <%--  <asp:BoundField DataField="PERIODO" HeaderText="Periodo" Visible="True" >
                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MONTO" HeaderText="Monto" Visible="True" >
                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                    </asp:BoundField>--%>
                                </Columns>
                                <PagerStyle CssClass ="GridHeader " />
                                <SelectedRowStyle CssClass="GridSelected" />
                                <HeaderStyle CssClass ="GridHeader" />
                                <PagerStyle CssClass="GridHeader" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>
                        </td>
                    </tr>
                    
                </table>
                <br />
                <div class="TooltipAdeudos">
                        <asp:TextBox ID="Txt_Documento" runat="server" style="width:100px;float:right;" ></asp:TextBox>
                    Documento
                    <br />
                        <asp:TextBox ID="Txt_Periodo_Corriente" runat="server" style="width:100px;float:right;" ></asp:TextBox>
                    Periodo corriente
                    <br />
                        <asp:TextBox ID="Txt_Monto_Corriente" runat="server" style="width:100px;float:right;" ></asp:TextBox>
                    Monto corriente
                    <br />
                        <asp:TextBox ID="Txt_Periodo_Rezago" runat="server" style="width:100px;float:right;" ></asp:TextBox>
                    Periodo rezago
                    <br />
                        <asp:TextBox ID="Txt_Monto_Rezago" runat="server" style="width:100px;float:right;" ></asp:TextBox>
                    Monto rezago
                    <br />
                        <asp:TextBox ID="Txt_Recargos_Ordinarios" runat="server" style="width:100px;float:right;" ></asp:TextBox>
                    Recargos ordinarios
                    <br />
                        <asp:TextBox ID="Txt_Recargos_Moratorios" runat="server" style="width:100px;float:right;" ></asp:TextBox>
                    Recargos moratorios
                    <br />
                        <asp:TextBox ID="Txt_Honorarios" runat="server" style="width:100px;float:right;" ></asp:TextBox>
                    Honorarios
                    <br />
                        <asp:TextBox ID="Txt_Multas" runat="server" style="width:100px;float:right;" ></asp:TextBox>
                    Multas
                    <br />
                        <asp:TextBox ID="Txt_Gastos_Ejecucion" runat="server" style="width:100px;float:right;" ></asp:TextBox>
                    Gastos de ejecución
                    <br />
                        <asp:TextBox ID="Txt_Descuento_Recargos" runat="server" style="width:100px;float:right;" ></asp:TextBox>
                    Descuento recargos
                    <br />
                        <asp:TextBox ID="Txt_Descuento_Honorarios" runat="server" style="width:100px;float:right;" ></asp:TextBox>
                    Descuento honorarios
                    <br />
                        <asp:TextBox ID="Txt_Descuento_Pronto_Pago" runat="server" style="width:100px;float:right;" ></asp:TextBox>
                    Descuento pronto pago
                </div>
                <br />
                
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

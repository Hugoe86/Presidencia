<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
 AutoEventWireup="true" CodeFile="Frm_Ope_Pre_PAE_Embargos.aspx.cs" Inherits="paginas_Predial_Frm_Ope_Pre_PAE_Embargos" Culture="es-MX"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">    
    <style type="text/css">
    body
    {
        font: normal 12px auto "Trebuchet MS", Verdana;    
        background-color: #ffffff;
        color: #4f6b72;       
    }

    .popUpStyle
    {        
        background-color: #000;   
        -moz-opacity: 0.50;
        filter: alpha(opacity=50);
    }
    
    .drag
    { 
        background-color: #FFFFFF;  
        border: solid 2px #5D7B9D;
        padding: 10px 10px 10px 10px;
    }
    
    .link
    {
    	color:Black;
    }
    .Label
    {
            width: 163px;
    }
    .TextBox
     {
        TEXT-ALIGN: right
     }
     
        </style>
    
      <!--SCRIPT PARA LA VALIDACION QUE NO EXPERE LA SESSION-->  
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
        setInterval('MantenSesion()', <%=(int)(0.9*(Session.Timeout * 60000))%>);

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
    <cc1:ToolkitScriptManager ID="Tsm_Embargos" runat="server"  AsyncPostBackTimeout="3600" EnableScriptGlobalization="true"/>
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0" >
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
                       <div id="Div_Contenido" style="background-color: #ffffff; width: 100%; height: 100%;">
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo">
                            Embargos
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png"
                                Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Encabezado_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                            <br />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error"></asp:Label>
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
                                            <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button"
                                                TabIndex="1" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" OnClick="Btn_Nuevo_Click" />
                                            <asp:ImageButton ID="Btn_Subir_Archivo" runat="server" ToolTip="Subir archivo" CssClass="Img_Button"
                                                TabIndex="5" ImageUrl="~/paginas/imagenes/paginas/sias_upload.png" />
                                            <asp:ImageButton ID="Btn_Imprimir" runat="server" ToolTip="Imprimir" CssClass="Img_Button"
                                                TabIndex="4" ImageUrl="~/paginas/imagenes/gridview/grid_print.png" 
                                                onclick="Btn_Imprimir_Click" />
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                                TabIndex="5" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Salir_Click" />
                                        </td>
                                        <td align="right" style="width: 41%;">
                                            <table style="width: 100%; height: 28px;">
                                                <tr>
                                                    <td style="vertical-align: middle; text-align: right; width: 20%;">
                                                        B&uacute;squeda:
                                                    </td>
                                                    <td style="width: 55%;">
                                                        <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100" TabIndex="5" ToolTip="Buscar"
                                                            Width="180px" />
                                                        <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                                                            WatermarkText="<Folio>" TargetControlID="Txt_Busqueda" />
                                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" runat="server" TargetControlID="Txt_Busqueda"
                                                            FilterType="Numbers" />
                                                    </td>
                                                    <td style="vertical-align: middle; width: 5%;">
                                                        <asp:ImageButton ID="Btn_Buscar" runat="server" TabIndex="6" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                                            ToolTip="Buscar" OnClick="Btn_Buscar_Click"/>
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
                <div id="Div_Generacion" runat="server">
                    <table width="98%" class="estilo_fuente">
                        <%---------------- Cuentas Rezago ----------------%>
                        <tr style="background-color: #36C;">
                            <td style="text-align: left; font-size: 15px; color: #FFF;" colspan="4">
                                Filtrar cuentas con Rezago
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Filtro por Despacho
                                </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:DropDownList ID="Cmb_Despacho_Filtro" runat="server" Width="90.4%" />
                            </td>
                        </tr>                       
                        <%---------------- Cuentas omitidas ----------------%>
                        <tr style="background-color: #36C;">
                            <td style="text-align: left; font-size: 15px; color: #FFF;" colspan="4">
                                Cuentas omitidas
                            </td>
                        </tr>
                        <tr align="center">
                            <td colspan="4">
                                <asp:GridView ID="Grid_Cuentas_Omitidas" runat="server" AllowPaging="True" CssClass="GridView_1"
                                    AutoGenerateColumns="False" PageSize="5" Width="100%" AllowSorting="True" HeaderStyle-CssClass="tblHead"
                                    Style="white-space: normal;" 
                                    onpageindexchanging="Grid_Cuentas_Omitidas_PageIndexChanging" 
                                    onselectedindexchanged="Grid_Cuentas_Omitidas_SelectedIndexChanged">
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                            <ItemStyle Width="4%" HorizontalAlign="Center" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="CUENTA_PREDIAL_ID" HeaderText="Cuenta_Id" Visible="false"/>
                                        <asp:BoundField DataField="CUENTA" HeaderText="Cuenta" HeaderStyle-Width="9%" >
                                            <HeaderStyle Width="9%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PERIODO_CORRIENTE" HeaderText="Periodo corriente" />
                                        <asp:BoundField DataField="CORRIENTE" HeaderText="Corriente" DataFormatString="{0:C2}"
                                            HeaderStyle-Width="7%">
                                            <HeaderStyle Width="7%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PERIODO_REZAGO" HeaderText="Periodo rezago" />
                                        <asp:BoundField DataField="REZAGO" HeaderText="Rezago" DataFormatString="{0:C2}"/>
                                        <asp:BoundField DataField="RECARGOS_ORDINARIOS" HeaderText="Recargos ordinarios" DataFormatString="{0:C2}"
                                            HeaderStyle-Width="10%" >
                                            <HeaderStyle Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="RECARGOS_MORATORIOS" HeaderText="Recargos moratorios" DataFormatString="{0:C2}"
                                            HeaderStyle-Width="10%" >
                                            <HeaderStyle Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="GASTOS_EJECUCION" HeaderText="Gastos ejecución" DataFormatString="{0:C2}"
                                            HeaderStyle-Width="10%" >
                                            <HeaderStyle Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="HONORARIOS" HeaderText="Honorarios" DataFormatString="{0:C2}"/>
                                        <asp:BoundField DataField="MULTAS" HeaderText="Multas" DataFormatString="{0:C2}"/>
                                        <asp:BoundField DataField="ADEUDO" HeaderText="Adeudo" DataFormatString="{0:C2}"/>
                                        <asp:BoundField DataField="MOTIVO_OMISIÓN" HeaderText="Motivo omisión" 
                                            HeaderStyle-Width="10%" >
                                            <HeaderStyle Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" Visible="false" />
                                    </Columns>
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <PagerStyle CssClass="GridHeader" />
                                    <HeaderStyle CssClass="tblHead" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;" colspan="2">
                                &nbsp;
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                Total omitidas
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Total_Adeudo_Omitidas" runat="server" Width="96.4%" 
                                    ReadOnly="True" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                &nbsp;
                            </td>
                        </tr>
                        <%---------------- Cuentas a determinar ----------------%>
                        <tr style="background-color: #36C;">
                            <td style="text-align: left; font-size: 15px; color: #FFF;" colspan="4">
                                Cuentas a embargar
                            </td>
                        </tr>
                        <tr align="center">
                            <td colspan="4">
                                <asp:GridView ID="Grid_Cuentas_Generar" runat="server" AllowPaging="True" CssClass="GridView_1"
                                    AutoGenerateColumns="False" PageSize="5" Width="100%" AllowSorting="True" HeaderStyle-CssClass="tblHead"
                                    Style="white-space: normal;" 
                                    OnPageIndexChanging="Grid_Cuentas_Generar_PageIndexChanging" 
                                    onselectedindexchanged="Grid_Cuentas_Generar_SelectedIndexChanged">
                                    <Columns>
                                        <asp:BoundField DataField="CUENTA_PREDIAL_ID" HeaderText="Cuenta_Id" Visible="false"/>
                                        <asp:BoundField DataField="CUENTA" HeaderText="Cuenta" HeaderStyle-Width="13%" />
                                        <asp:BoundField DataField="PERIODO_CORRIENTE" HeaderText="Periodo corriente" />
                                        <asp:BoundField DataField="CORRIENTE" HeaderText="Corriente" DataFormatString="{0:C2}"/>
                                        <asp:BoundField DataField="PERIODO_REZAGO" HeaderText="Periodo rezago" />
                                        <asp:BoundField DataField="REZAGO" HeaderText="Rezago" DataFormatString="{0:C2}"/>
                                        <asp:BoundField DataField="RECARGOS_ORDINARIOS" HeaderText="Recargos ordinarios" DataFormatString="{0:C2}"
                                            HeaderStyle-Width="10%" />
                                        <asp:BoundField DataField="RECARGOS_MORATORIOS" HeaderText="Recargos moratorios" DataFormatString="{0:C2}"
                                            HeaderStyle-Width="10%" />
                                        <asp:BoundField DataField="HONORARIOS" HeaderText="Honorarios" DataFormatString="{0:C2}"/>
                                        <asp:BoundField DataField="GASTOS_EJECUCION" HeaderText="Gastos ejecución" DataFormatString="{0:C2}" />                                            
                                        <asp:BoundField DataField="MULTAS" HeaderText="Multas" DataFormatString="{0:C2}"/>
                                        <asp:BoundField DataField="ADEUDO" HeaderText="Adeudo" DataFormatString="{0:C2}"/>
<%--                                        <asp:ButtonField ButtonType="Btn_MotivossSs" CommandName="Select" ImageUrl="~/paginas/imagenes/paginas/delete.png">
                                            <ItemStyle Width="5%" HorizontalAlign="Center" />
                                        </asp:ButtonField>--%>
                                        <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="Btn_Eliminar_Movimiento" runat="server" Height="20px" 
                                                            ImageUrl="~/paginas/imagenes/paginas/delete.png" TabIndex="10"
                                                            CommandName="Select"                                                          
                                                            ToolTip="Omitir Cuenta" Width="20px"
                                                            OnClientClick="Abrir_Ventana_Modal('Ventanas_Emergentes/PAE/Frm_Motivo_Omision.aspx', 'center:yes;resizable:yes;status:no;dialogWidth:275px;dialogHeight:150px;dialogHide:true;help:no;scroll:no');"
                                                            />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" Visible="false" />
                                    </Columns>
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <PagerStyle CssClass="GridHeader" />
                                    <HeaderStyle CssClass="tblHead" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>
                                <asp:Button ID="Btn_Auxiliar" runat="server" Style="display: none" CausesValidation="false"/>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;" colspan="2">
                                &nbsp;
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                Total a determinar
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Adeudo_Determinar" runat="server" Width="96.4%" 
                                    ReadOnly="True" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                *Asignar
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:DropDownList ID="Cmb_Despachos" runat="server" Width="99%" TabIndex="7" AutoPostBack="true"
                                    onselectedindexchanged="Cmb_Despachos_SelectedIndexChanged">
                                    <asp:ListItem> Municipio </asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                Número de entrega
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_No_Entrega" runat="server" Width="96.4%" ReadOnly="True" />
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_No_Entrega" runat="server" TargetControlID="Txt_No_Entrega"
                                    FilterType="Numbers" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%; vertical-align: top;">
                                Comentarios
                            </td>
                            <td colspan="3" style="text-align: left; width: 80%; vertical-align: top;">
                                <asp:TextBox ID="Txt_Comentarios" runat="server" TabIndex="10" MaxLength="250" TextMode="MultiLine" onkeyup='this.value=this.value.toUpperCase();'
                                    Width="98.6%" AutoPostBack="True" />
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Comentarios" runat="server" TargetControlID="Txt_Comentarios"
                                    WatermarkText="Límite de Caracteres 250" WatermarkCssClass="watermarked" />
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Comentarios" runat="server" TargetControlID="Txt_Comentarios"
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="Div_Generadas" runat="server">
                    <table width="98%" class="estilo_fuente">
                        <caption>
                            <%---------------- Determinaciones generadas ----------------%>
                            <tr style="background-color: #36C;">
                                <td colspan="4" style="text-align: left; font-size: 15px; color: #FFF;">
                                    Embargos generados
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left; width: 20%;">
                                    Folio inicial
                                </td>
                                <td style="text-align: left; width: 30%;">
                                    <asp:TextBox ID="Txt_Folio_Inicial" runat="server" Width="96.4%" />
                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Folio_Inicial" runat="server" TargetControlID="Txt_Folio_Inicial"
                                        FilterType="Numbers" />
                                </td>
                                <td style="text-align: left; width: 20%; text-align: right;">
                                    Folio final
                                </td>
                                <td style="text-align: left; width: 30%;">
                                    <asp:TextBox ID="Txt_Folio_Final" runat="server" Width="96.4%" />
                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Folio_Final" runat="server" TargetControlID="Txt_Folio_Final"
                                        FilterType="Numbers" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left; width: 20%;">
                                    Propietario
                                </td>
                                <td style="text-align: left; width: 30%;">
                                    <asp:TextBox ID="Txt_Propietario" runat="server" Width="87%" Enabled="False" />
                                    <asp:ImageButton ID="Btn_Seleccionar_Propietario" runat="server" Height="22px" 
                                        ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png"
                                        TabIndex="10" 
                                        ToolTip="Seleccionar Propietario" Width="22px" 
                                        OnClick="Btn_Seleccionar_Propietario_Click"  
                                        />
                                </td>
                                <td style="text-align: left; width: 20%; text-align: right;">
                                    Cuenta predial
                                </td>
                                <td style="text-align: left; width: 30%;">
                                    <asp:TextBox ID="Txt_Cuenta_Predial" runat="server" Width="96.4%" onkeyup='this.value=this.value.toUpperCase();'/>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left; width: 20%;">
                                    Fecha inicial
                                </td>
                                <td style="text-align: left; width: 30%;">
                                    <asp:TextBox ID="Txt_Fecha_Inicial" runat="server" Height="18px" MaxLength="11" TabIndex="12" AutoPostBack="true" OnTextChanged="Txt_Fecha_Inicial_TextChanged"
                                        Width="84%" />
                                    <cc1:TextBoxWatermarkExtender ID="TBWE_Txt_Fecha_Inicial" runat="server" Enabled="True"
                                        TargetControlID="Txt_Fecha_Inicial" WatermarkCssClass="watermarked" WatermarkText="Dia/Mes/Año" />
                                    <cc1:CalendarExtender ID="CE_Txt_Fecha_Inicial" runat="server" Enabled="True" Format="dd/MMM/yyyy"
                                        PopupButtonID="Btn_Txt_Fecha_Inicial" TargetControlID="Txt_Fecha_Inicial" />
                                    <asp:ImageButton ID="Btn_Txt_Fecha_Inicial" runat="server" CausesValidation="false"
                                        Height="18px" ImageUrl="../imagenes/paginas/SmallCalendar.gif" Style="vertical-align: top;" />
                                </td>                                
                                <td style="text-align: left; width: 20%; text-align: right;">
                                    Fecha final
                                </td>
                                <td style="text-align: left; width: 30%;">
                                    <asp:TextBox ID="Txt_Fecha_Final" runat="server" Height="18px" MaxLength="11" TabIndex="12" AutoPostBack="true" OnTextChanged="Txt_Fecha_Final_TextChanged"
                                        Width="84%" />
                                    <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Fecha_Final" runat="server" Enabled="True"
                                        TargetControlID="Txt_Fecha_Final" WatermarkCssClass="watermarked" WatermarkText="Dia/Mes/Año" />
                                    <cc1:CalendarExtender ID="CE_Txt_Fecha_Final" runat="server" Enabled="True" Format="dd/MMM/yyyy"
                                        PopupButtonID="Btn_Txt_Fecha_Final" TargetControlID="Txt_Fecha_Final" />
                                    <asp:ImageButton ID="Btn_Txt_Fecha_Final" runat="server" CausesValidation="false"
                                        Height="18px" ImageUrl="../imagenes/paginas/SmallCalendar.gif" Style="vertical-align: top;" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left; width: 20%;">
                                    Domicilio
                                </td>
                                <td style="text-align: left; width: 30%;">
                                    <asp:TextBox ID="Txt_Domicilio" runat="server" Width="87%" />
                                    <asp:ImageButton ID="Btn_Busca_Domicilio" runat="server" Height="22px" 
                                        ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" 
                                        onclick="Btn_Busca_Domicilio_Click" Width="22px" />
                                </td>
                                <td style="text-align: left; width: 20%; text-align: right;">
                                    Tipo domicilio
                                </td>
                                <td style="text-align: left; width: 30%;">
                                    <asp:DropDownList ID="Cmb_Tipo_Domicilio" runat="server" TabIndex="7" 
                                        Width="99%"> 
                                        <asp:ListItem Text="&lt;--SELECCIONE--&gt;" Value="0" />
                                        <asp:ListItem Text="UBICACIÓN" Value="1" />
                                        <asp:ListItem Text="NOTIFICACIÓN" Value="2" />
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left; width: 20%;">
                                    Tipo de predio
                                </td>
                                <td style="text-align: left; width: 30%;">
                                    <asp:DropDownList ID="Cmb_Tipo_Predio" runat="server" TabIndex="7" Width="99%">
                                        <asp:ListItem Text="&lt;SELECCIONE&gt;" Value="SELECCIONE" />
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align: left; width: 20%; text-align: right;">
                                    Asignado a
                                </td>
                                <td style="text-align: left; width: 30%;">
                                    <asp:DropDownList ID="Cmb_Asignado_a" runat="server" TabIndex="7" Width="99%">
                                        <asp:ListItem Text="&lt;SELECCIONE&gt;" Value="SELECCIONE" />
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left; width: 20%;">
                                    Estatus
                                </td>
                                <td style="text-align: left; width: 30%;">
                                    <asp:DropDownList ID="Cmb_Estatus" runat="server" TabIndex="7" Width="99%">
                                        <asp:ListItem Text="&lt;--SELECCIONE--&gt;" Value="0" />
                                        <asp:ListItem Text="NOTIFICACION" Value="1" />
                                        <asp:ListItem Text="NO DILIGENCIADO" Value="2" />
                                        <asp:ListItem Text="ILOCALIZABLE" Value="3" />
                                        <asp:ListItem Text="PENDIENTE" Value="4" />
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align: left; width: 20%; text-align: right;">
                                    Ordenar
                                </td>
                                <td style="text-align: left; width: 30%;">
                                    <asp:DropDownList ID="Cmb_Ordenar" runat="server" TabIndex="7" Width="99%">
                                        <asp:ListItem> POR ZONA </asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left; width: 20%;">
                                    Formato
                                </td>
                                <td style="text-align: left; width: 30%;">
                                    <asp:DropDownList ID="Cmb_Formato" runat="server" TabIndex="7" Width="99%">
                                        <asp:ListItem> ESTRATEGICOS </asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td colspan="2" style="text-align: left; width: 20%; text-align: right;">
                                    <asp:Label ID="Lbl_Busqueda" runat="server" Text="Buscar Embargos"></asp:Label>
                                    <asp:ImageButton ID="Btn_Buscar_Embargos" runat="server" 
                                        ImageUrl="~/paginas/imagenes/paginas/icono_consultar.png" 
                                        onclick="Btn_Buscar_Embargos_Click" />
                                    &nbsp;
                                </td>
                            </tr>
                            <tr align="center">
                                <td colspan="4">
                                    <asp:GridView ID="Grid_Embargos_Generadas" runat="server" 
                                        AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                                        CssClass="GridView_1" HeaderStyle-CssClass="tblHead" PageSize="5" Style="white-space: normal;"
                                        Width="100%" 
                                        onpageindexchanging="Grid_Embargos_Generadas_PageIndexChanging">
                                        <Columns>
                                            <asp:BoundField DataField="CUENTA" HeaderText="Cuenta">
                                                <ItemStyle HorizontalAlign="Center"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ADEUDO" HeaderText="Adeudo" DataFormatString="{0:C2}">
                                                <ItemStyle HorizontalAlign="right"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="FECHA" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}">
                                                <ItemStyle HorizontalAlign="Center"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="FOLIO" HeaderText="Folio">
                                                <ItemStyle HorizontalAlign="Center"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ASIGNADO" HeaderText="Asignado" >
                                                <ItemStyle HorizontalAlign="Center"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ENTREGA" HeaderText="Entrega" >
                                                <ItemStyle HorizontalAlign="Center"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" >
                                                <ItemStyle HorizontalAlign="Center"/>
                                            </asp:BoundField>
                                        </Columns>
                                        <SelectedRowStyle CssClass="GridSelected" />
                                        <PagerStyle CssClass="GridHeader" />
                                        <HeaderStyle CssClass="tblHead" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                    </asp:GridView>
                                </td>
                            </tr>                            
                        </caption>
                    </table> 
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                </div>
                <br />
                <asp:HiddenField ID="Hdn_Colonia_ID" runat="server" />
                <asp:HiddenField ID="Hdn_Modo_Generacion" runat="server" Value="Normal" />
                <asp:HiddenField ID="Hdn_Calle_ID" runat="server" />
                <asp:HiddenField ID="Hdn_Estatus" runat="server" />
                <asp:HiddenField ID="Hdn_Motivo_Omision" runat="server" />
                <asp:HiddenField ID="Hdn_Contribuyente_Id" runat="server" />
                <asp:HiddenField ID="Hdn_Contribuyente_RFC" runat="server" />
                <asp:HiddenField ID="Hdn_Cuenta_ID" runat="server" />
                <asp:Button ID="Btn_Relaciona_Modal" runat="server" Style="display: none" CausesValidation="false"/>
                <asp:Button ID="Btn_Relaciona_Modal_2" runat="server" Style="display: none" CausesValidation="false"/>
<%--                <cc1:ModalPopupExtender ID="Mpe_Subir_Archivo" runat="server"
                    TargetControlID="Btn_Relaciona_Modal_2"
                    PopupControlID="Pnl_Subir_Archivo" 
                    CancelControlID="Btn_Cancelar_Carga"                   
                    BackgroundCssClass="popUpStyle"
                    DropShadow="true">
                </cc1:ModalPopupExtender>--%>
                <asp:Panel ID="Pnl_Subir_Archivo" runat="server" Style="display:none;border-style:outset;border-color:Silver;background-image:url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG);background-repeat:repeat-y;max-width:850px;"  CssClass="drag">
                    <asp:Label ID="Lbl_Subir" runat="server" Text="Subir Archivo" />
                    <br />
                    <br />
                    <asp:FileUpload ID="Fle_Cargar_Archivo" runat="server" />
                    <br />
                    <br />
                    <asp:Button ID="Btn_Cargar_Archivo" runat="server" Text="Aceptar" />                    
                    <asp:Button ID="Btn_Cancelar_Carga" runat="server" Text="Cancelar"/>
                    <asp:GridView ID="Grid_Cargar_Cuentas" runat="server"
                        AllowPaging="true">
                    </asp:GridView>
                </asp:Panel>
                </ContentTemplate>        
    </asp:UpdatePanel>
</asp:Content>
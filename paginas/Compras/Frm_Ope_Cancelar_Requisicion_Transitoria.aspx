<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Cancelar_Requisicion_Transitoria.aspx.cs" 
    Inherits="paginas_Compras_Frm_Ope_Cancelar_Requisicion_Transitoria"
    Title="Cancelar Requisicion Transitoria" Culture="es-MX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">
        function calendarShown(sender, args){
            sender._popupBehavior._element.style.zIndex = 10000005;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">
   <!--SCRIPT PARA LA VALIDACION QUE NO EXPiRE LA SESSION-->  
   <script language="javascript" type="text/javascript">
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
   </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true">
    </asp:ScriptManager>
    <div id="Div_General" style="width: 98%;" visible="true" runat="server">
        <asp:UpdatePanel ID="Upl_Contenedor" runat="server">
            <ContentTemplate>
                <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upl_Contenedor"
                    DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                        </div>
                        <div class="processMessage" id="div_progress">
                            <img alt="" src="../Imagenes/paginas/Updating.gif" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <%--Div Encabezado--%>
                <div id="Div_Encabezado" runat="server">
                    <table style="width: 100%;" border="0" cellspacing="0">
                        <tr align="center">
                            <td colspan="4" class="label_titulo">
                                Cancelar Requisición Transitoria
                            </td>
                        </tr>
                        <tr align="left">
                            <td colspan="4">
                                <asp:Image ID="Img_Warning" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png"
                                    Visible="false" />
                                <asp:Label ID="Lbl_Informacion" runat="server" ForeColor="#990000"></asp:Label>
                                <asp:HiddenField ID="Hdn_Listado_Almacen" runat="server" />
                                
                            </td>
                        </tr>
                        <tr class="barra_busqueda" align="right">
                            <td align="left" valign="middle" colspan="2">
                                <asp:ImageButton ID="Btn_Nuevo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"
                                    CssClass="Img_Button" ToolTip="Nuevo" OnClick="Btn_Nuevo_Click" />
                                <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                    CssClass="Img_Button" AlternateText="Modificar" ToolTip="Modificar" OnClick="Btn_Modificar_Click" />
                                <asp:ImageButton ID="Btn_Eliminar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"
                                    CssClass="Img_Button" AlternateText="Eliminar" ToolTip="Eliminar" />
                                <asp:ImageButton ID="Btn_Listar_Requisiciones" runat="server" CssClass="Img_Button"
                                    ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" ToolTip="Listar Requisiciones"
                                    OnClick="Btn_Listar_Requisiciones_Click" />
                                <asp:ImageButton ID="Btn_Imprimir_Req" runat="server" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/gridview/grid_print.png"
                                    ToolTip="Imprimir Requisición" OnClick="Btn_Imprimir_Req_Click" />
                                <asp:ImageButton ID="Btn_Salir" runat="server" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                    ToolTip="Inicio" OnClick="Btn_Salir_Click" />
                            </td>
                            <td colspan="2">
                            </td>
                        </tr>
                    </table>
                </div>
                <%--Div listado de requisiciones--%>
                <div id="Div_Listado_Requisiciones" runat="server">
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 15%;">
                                Unidad Responsable
                            </td >
                            <td colspan="3">
                                <asp:DropDownList ID="Cmb_Dependencia_Panel" runat="server" Width="98%" />
                            </td>
                        </tr>
                        <tr >
                            <td style="width: 15%;">
                                Fecha
                            </td>
                            <td style="width: 35%;">
                                <asp:TextBox ID="Txt_Fecha_Inicial" runat="server" Width="85px" Enabled="false"></asp:TextBox>
                                <cc1:filteredtextboxextender id="Txt_Fecha_Inicial_FilteredTextBoxExtender" runat="server"
                                    targetcontrolid="Txt_Fecha_Inicial" filtertype="Custom, Numbers, LowercaseLetters, UppercaseLetters"
                                    validchars="/_" />
                                <cc1:calendarextender id="Txt_Fecha_Inicial_CalendarExtender" runat="server" targetcontrolid="Txt_Fecha_Inicial"
                                    popupbuttonid="Btn_Fecha_Inicial" format="dd/MMM/yyyy" />
                                <asp:ImageButton ID="Btn_Fecha_Inicial" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                    ToolTip="Seleccione la Fecha Inicial" />
                                :&nbsp;&nbsp;
                                <asp:TextBox ID="Txt_Fecha_Final" runat="server" Width="85px" Enabled="false"></asp:TextBox>
                                <cc1:calendarextender id="CalendarExtender3" runat="server" targetcontrolid="Txt_Fecha_Final"
                                    popupbuttonid="Btn_Fecha_Final" format="dd/MMM/yyyy" />
                                <asp:ImageButton ID="Btn_Fecha_Final" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                    ToolTip="Seleccione la Fecha Final" />
                            </td>
                            <td style="width: 15%; text-align:right;">
                                Folio
                            </td>
                            <td style="width: 35%; text-align:left">
                                <asp:TextBox ID="Txt_Busqueda" runat="server" Width="65%" MaxLength="13" 
                                    AutoPostBack="true" ontextchanged="Txt_Busqueda_TextChanged"></asp:TextBox>
                                <asp:ImageButton ID="Btn_Buscar" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                    OnClick="Btn_Buscar_Click" ToolTip="Consultar" />
                                <cc1:textboxwatermarkextender id="TextBoxWatermarkExtender1" runat="server" targetcontrolid="Txt_Busqueda"
                                    watermarktext="&lt;RQ-000000&gt;" watermarkcssclass="watermarked">
                                </cc1:textboxwatermarkextender>
                                <cc1:filteredtextboxextender id="FTBE_Txt_Busqueda" runat="server" filtertype="Custom"
                                    targetcontrolid="Txt_Busqueda" validchars="rRqQ-0123456789">
                                </cc1:filteredtextboxextender>
                            </td>  
                        </tr>                    
                        
                        <tr style="display:none;">
                            <td style="width: 15%;">
                                Tipo
                            </td>
                            <td>
                                <asp:DropDownList ID="Cmb_Tipo_Busqueda" runat="server" Width="98%" />
                            </td>
                        </tr>
                        <tr style="display:none;">
                            <td>
                                Estatus
                            </td>
                            <td>
                                <asp:DropDownList ID="Cmb_Estatus_Busqueda" runat="server" Width="98%" />
                            </td>

                        </tr>

                        <tr>
                            <td style="width: 99%" align="center" colspan="4">
                                <div style="overflow: auto; height: 320px; width: 99%; vertical-align: top; border-style: outset;
                                    border-color: Silver;">
                                    <asp:GridView ID="Grid_Requisiciones" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                                        CssClass="GridView_1" GridLines="None" Width="100%" OnPageIndexChanging="Grid_Requisiciones_PageIndexChanging"
                                        OnSelectedIndexChanged="Grid_Requisiciones_SelectedIndexChanged" DataKeyNames="No_Requisicion"
                                        AllowSorting="true" HeaderStyle-CssClass="tblHead" OnSorting="Grid_Requisiciones_Sorting"
                                        OnRowDataBound="Grid_Requisiciones_RowDataBound" Font-Size="X-Small" EmptyDataText="Lista de Requisiciones">
                                        <RowStyle CssClass="GridItem" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="Btn_Seleccionar_Requisicion" runat="server" ImageUrl="~/paginas/imagenes/gridview/blue_button.png"
                                                        OnClick="Btn_Seleccionar_Requisicion_Click" CommandArgument='<%# Eval("No_Requisicion") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" Width="3%" />
                                                <ItemStyle HorizontalAlign="Center" Width="3%" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Folio" HeaderText="Folio" Visible="True" SortExpression="No_Requisicion">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" Width="11%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Fecha_Creo" HeaderText="Fecha Inicial" DataFormatString="{0:dd/MMM/yyyy}"
                                                Visible="True" SortExpression="Fecha_Creo">
                                                <HeaderStyle HorizontalAlign="Left" Wrap="true" />
                                                <ItemStyle HorizontalAlign="Left" Width="11%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="NOMBRE_DEPENDENCIA" HeaderText="Unidad Responsable" Visible="True"
                                                SortExpression="NOMBRE_DEPENDENCIA">
                                                <FooterStyle HorizontalAlign="Left" />
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Tipo" HeaderText="Tipo" Visible="True" SortExpression="Tipo">
                                                <FooterStyle HorizontalAlign="Left" />
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" Width="12%" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="Btn_Alerta" runat="server" ImageUrl="~/paginas/imagenes/gridview/circle_grey.png"
                                                        CommandArgument='<%# Eval("No_Requisicion") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" Width="3%" />
                                                <ItemStyle HorizontalAlign="Center" Width="3%" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Estatus" HeaderText="Estatus" Visible="True" SortExpression="Estatus">
                                                <FooterStyle HorizontalAlign="Left" />
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" Width="16%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="No_Requisicion" HeaderText="ID" Visible="false">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ALERTA" HeaderText="Alerta" Visible="false" SortExpression="Estatus">
                                                <FooterStyle HorizontalAlign="Left" />
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                        </Columns>
                                        <PagerStyle CssClass="GridHeader" />
                                        <SelectedRowStyle CssClass="GridSelected" />
                                        <HeaderStyle CssClass="GridHeader" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <%--Div Contenido--%>
                <div id="Div_Contenido" runat="server">
                    <asp:Panel ID="Pnl_Datos_Generales" runat="server" GroupingText="Datos de Requisición"
                        Width="100%">
                        <table style="width: 100%;">
                            <tr>
                                <td style="width: 16%;" align="left">
                                    Folio
                                </td>
                                <td style="width: 34%;" align="left">
                                    <asp:TextBox ID="Txt_Folio" runat="server" Width="96%"></asp:TextBox>
                                </td>
                                <td style="width: 15%;" align="center">
                                   Fecha
                                </td>
                                <td style="width: 35%;" align="left">
                                    <asp:TextBox ID="Txt_Fecha" runat="server" Width="96%" />
                                </td>   
                            </tr>
                            <tr>
                                <td style="width: 16%;" align="left">
                                    Unidad Responsable
                                </td>
                                <td style="width: 34%;" align="left" colspan="3">
                                    <asp:DropDownList ID="Cmb_Dependencia" runat="server" Width="99%" OnSelectedIndexChanged="Cmb_Dependencia_SelectedIndexChanged"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                             </tr>
                             <tr>
                                <td style="width: 15%;" align="left">
                                    Tipo Requisición
                                </td>
                                <td style="width: 35%;" align="left">
                                    <asp:DropDownList ID="Cmb_Tipo" runat="server" Width="58%" AutoPostBack="True" OnSelectedIndexChanged="Cmb_Tipo_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="Cmb_Producto_Servicio" runat="server" Width="40%" OnSelectedIndexChanged="Cmb_Producto_Servicio_SelectedIndexChanged"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 15%;" align="center">
                                    Estatus
                                </td>
                                <td style="width: 35%;" align="left">
                                    <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="98%" />
                                    
                                </td>                                
                            </tr>
                            <tr style="display:none;">
                                <td style="width: 16%;" align="left">
                                    *Fte. Financiamiento
                                </td>
                                <td style="width: 34%;" align="left">
                                    <asp:DropDownList ID="Cmb_Fte_Financiamiento" runat="server" Width="98%" OnSelectedIndexChanged="Cmb_Fte_Financiamiento_SelectedIndexChanged"
                                        AutoPostBack="true" />
                                </td>
                                <td style="width: 15%;" align="left">
                                    *Programa
                                </td>
                                <td style="width: 35%;" align="left">
                                    <asp:DropDownList ID="Cmb_Programa" runat="server" Width="99%" OnSelectedIndexChanged="Cmb_Programa_SelectedIndexChanged"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr style="display:none;">
                                <td style="width: 16%;" align="left" valign="top">
                                    *Partida
                                </td>
                                <td style="width: 34%;" align="left" valign="top">
                                    <asp:DropDownList ID="Cmb_Partida" runat="server" Width="99%" AutoPostBack="true"
                                        OnSelectedIndexChanged="Cmb_Partida_SelectedIndexChanged1">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 15%;" align="left">
                                    Disponible
                                </td>
                                <td style="width: 35%;" align="left">
                                    <asp:Label ID="Lbl_Disponible_Partida" runat="server" Text=" $ 0.00" ForeColor="Blue"
                                        BorderColor="Blue" BorderWidth="2px" Width="98%">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 15%;" align="left">
                            </td>
                            <td style="width: 85%;" align="left" colspan="3">
                                <div id="Div_Presupuesto" runat="server" visible="false">
                                    <table border="2" width="99%">
                                        <tr style="background-color: #D8D8D8; height: 3px;">
                                            <td>
                                                Partida
                                            </td>
                                            <td>
                                                Clave
                                            </td>
                                            <td>
                                                Disponible
                                            </td>
                                            <td>
                                                Fecha Asignación
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 45%">
                                                <asp:Label ID="Lbl_Partida" runat="server" Text="Label" ForeColor="Green"></asp:Label>
                                            </td>
                                            <td style="width: 15%">
                                                <asp:Label ID="Lbl_Clave" runat="server" Text="Label" ForeColor="Green"></asp:Label>
                                            </td>
                                            <td style="width: 20%">
                                                <asp:Label ID="Lbl_Disponible" runat="server" Text="Label" ForeColor="Green"></asp:Label>
                                            </td>
                                            <td style="width: 20%">
                                                <asp:Label ID="Lbl_Fecha_Asignacion" runat="server" Text="Label" ForeColor="Green"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <asp:Panel ID="Pnl_Busqueda_Prod_Serv" runat="server" GroupingText="Búsqueda Productos - Servicios" Visible="false"
                        Width="100%">
                        <table style="width: 100%;">
                            <tr style="display:none;">
                                <td style="width: 15%;" align="left">
                                    <asp:Label ID="Lbl_Categoria" runat="server" Text="Producto/Servicio"></asp:Label>
                                </td>
                                <td style="width: 55%;" align="left" valign="middle" colspan="2">
                                    <asp:TextBox ID="Txt_Producto_Servicio" runat="server" Width="98%"></asp:TextBox>
                                </td>
                                <td style="width: 35%;" align="left" valign="middle">
                                    <table>
                                        <tr>
                                            <td style="width: 6%" align="left">
                                                <asp:ImageButton ID="Ibtn_Buscar_Producto" runat="server" CssClass="Img_Button" OnClick="Ibtn_Buscar_Producto_Click"
                                                    ToolTip="Búscar producto ó servicio" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" />
                                            </td>
                                            <td style="width: 41%" align="right">
                                                *Cantidad
                                            </td>
                                            <td style="width: 47%;" align="left">
                                                <asp:TextBox ID="Txt_Cantidad" runat="server" Width="96%" MaxLength="8"></asp:TextBox>
                                                <cc1:filteredtextboxextender id="Ftbe_Cantidad" runat="server" targetcontrolid="Txt_Cantidad"
                                                    filtertype="Numbers">
                                            </cc1:filteredtextboxextender>
                                            </td>
                                            <td style="width: 4%" align="right">
                                                <asp:ImageButton ID="Ibtn_Agregar_Producto" runat="server" OnClick="Ibtn_Agregar_Producto_Click"
                                                    ToolTip="Agregar producto ó servicio" ImageUrl="~/paginas/imagenes/paginas/accept.png" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%;" align="left">
                                </td>
                                <td style="width: 55%;" align="left" colspan="2">
                                    <asp:Label ID="Lbl_Disponible_Producto" runat="server" Text="Disponible: 0 / Precio aproximado: $ 0.00"
                                        ForeColor="Blue" BorderColor="Blue" BorderWidth="2px" Width="98%">
                                    </asp:Label>
                                </td>
                                <td style="width: 35%;" align="left" valign="middle">
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <table style="width: 100%;">
                        <tr>
                            <td align="center" colspan="4">
                                <div id="Div_Partidas_Productos_Tmp" runat="server" visible="false">
                                    <table>
                                        <tr>
                                            <td align="center" colspan="4">
                                                <asp:GridView ID="Grid_Partidas_Tmp" runat="server" AutoGenerateColumns="false" CssClass="GridView_1"
                                                    AllowPaging="True" PageSize="5" GridLines="None" Width="100%">
                                                    <RowStyle CssClass="GridItem" />
                                                    <Columns>
                                                        <asp:BoundField DataField="PARTIDA_ID" HeaderText="PARTIDA_ID" Visible="false">
                                                            <FooterStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="CLAVE" HeaderText="Clave" Visible="true">
                                                            <FooterStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="MONTO_DISPONIBLE" HeaderText="Disponible" Visible="true">
                                                            <FooterStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="MONTO_COMPROMETIDO" HeaderText="Comprometido" Visible="true">
                                                            <FooterStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                    <PagerStyle CssClass="GridHeader" />
                                                    <SelectedRowStyle CssClass="GridSelected" />
                                                    <HeaderStyle CssClass="GridHeader" />
                                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                                </asp:GridView>
                                            </td>
                                            <%--                                        </tr>
                                        <tr>--%>
                                            <td align="center" colspan="4">
                                                <asp:GridView ID="Grid_Productos_Tmp" runat="server" AutoGenerateColumns="false"
                                                    CssClass="GridView_1" AllowPaging="True" PageSize="5" GridLines="None" Width="100%">
                                                    <RowStyle CssClass="GridItem" />
                                                    <Columns>
                                                        <asp:BoundField DataField="PRODUCTO_ID" HeaderText="Producto" Visible="false">
                                                            <FooterStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="CLAVE" HeaderText="Clave" Visible="true">
                                                            <FooterStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" Visible="true">
                                                            <FooterStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="DISPONIBLE" HeaderText="Disponible" Visible="true">
                                                            <FooterStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="COMPROMETIDO" HeaderText="Comprometido" Visible="true">
                                                            <FooterStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                    <PagerStyle CssClass="GridHeader" />
                                                    <SelectedRowStyle CssClass="GridSelected" />
                                                    <HeaderStyle CssClass="GridHeader" />
                                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                        </tr>
                        <tr style="background-color: #3366CC">
                            <td style="text-align: left; font-size: 15px; color: #FFFFFF; height: 1px;" colspan="4">
                            </td>
                        </tr>
                    </table>
                    <table style="width: 100%;">
                        <tr>
                            <td align="center" colspan="4">
                                <asp:GridView ID="Grid_Productos_Servicios" runat="server" AutoGenerateColumns="False"
                                    CssClass="GridView_1" AllowPaging="false" PageSize="5" GridLines="None" Width="100%"
                                    OnRowCommand="Grid_Productos_Servicios_RowCommand" DataKeyNames="Tipo,Prod_Serv_ID,Monto_IVA,Monto_IEPS"
                                    OnPageIndexChanging="Grid_Productos_Servicios_PageIndexChanging" Style="white-space: normal">
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Eliminar" ImageUrl="~/paginas/imagenes/gridview/grid_garbage.png" Visible="false">
                                            <ItemStyle Width="4%" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="Clave" HeaderText="Clave" Visible="True">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="8%" Font-Size="X-Small" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Nombre_Producto_Servicio" HeaderText="Producto/Servicio"
                                            Visible="True">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Cantidad" HeaderText="Ctd." Visible="True">
                                            <FooterStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="6%" Font-Size="X-Small" />
                                        </asp:BoundField>
                                        <asp:ButtonField ButtonType="Image" ImageUrl="~/paginas/imagenes/gridview/add_grid.png"
                                            CommandName="Mas" HeaderText="" ItemStyle-Width="2.5%" Visible ="false" >
                                            <ItemStyle Width="2%" Font-Size="X-Small" />
                                        </asp:ButtonField>
                                        <asp:ButtonField ButtonType="Image" CommandName="Menos" HeaderText="" ImageUrl="~/paginas/imagenes/gridview/minus_grid.png"
                                            ItemStyle-Width="2.5%" Visible ="false">
                                            <ItemStyle Width="2%" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="Precio_Unitario" HeaderText="$ Unitario" Visible="True"
                                            DataFormatString="{0:C}">
                                            <FooterStyle HorizontalAlign="Right" />
                                            <HeaderStyle HorizontalAlign="Right" />
                                            <ItemStyle HorizontalAlign="Right" Width="12%" Font-Size="X-Small" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Importe" HeaderText="Acumulado C/I" Visible="false">
                                            <FooterStyle HorizontalAlign="Right" />
                                            <HeaderStyle HorizontalAlign="Right" Wrap="true" />
                                            <ItemStyle HorizontalAlign="Right" Width="0%" Font-Size="X-Small" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Monto_IVA" HeaderText="IVA" Visible="False">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="0%" Font-Size="X-Small" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Monto_IEPS" HeaderText="IEPS" Visible="False">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="0%" Font-Size="X-Small" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Porcentaje_IEPS" HeaderText="Porcentaje_IEPS" Visible="False">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="0%" Font-Size="X-Small" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Porcentaje_IVA" HeaderText="Porcentaje_IVA" Visible="False">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="0%" Font-Size="X-Small" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Monto" HeaderText="$ Importe" Visible="true" DataFormatString="{0:C}">
                                            <FooterStyle HorizontalAlign="Right" />
                                            <HeaderStyle HorizontalAlign="Right" />
                                            <ItemStyle HorizontalAlign="Right" Width="12%" Font-Size="X-Small" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Prod_Serv_ID" HeaderText="Producto_Servicio_ID" Visible="False">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="0%" Font-Size="X-Small" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Partida" HeaderText="Partida" Visible="False">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="0%" Font-Size="X-Small" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Proyecto_Programa" HeaderText="Proyecto_Programa" Visible="False">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="0%" Font-Size="X-Small" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Tipo" HeaderText="Tipo" Visible="false">
                                            <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
                                        </asp:BoundField>
                                    </Columns>
                                    <PagerStyle CssClass="GridHeader" />
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <HeaderStyle CssClass="GridHeader" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="4">
                                Subtotal
                                <asp:TextBox ID="Txt_Subtotal" runat="server" Style="text-align: right" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="4">
                                IEPS
                                <asp:TextBox ID="Txt_IEPS" runat="server" Style="text-align: right" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="4">
                                IVA
                                <asp:TextBox ID="Txt_IVA" runat="server" Style="text-align: right" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <%--                            <td align="right" colspan="3">
                            </td>--%>
                            <td align="right" colspan="4">
                                Total
                                <asp:TextBox ID="Txt_Total" runat="server" Style="text-align: right"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="4">
                                <hr class="linea" />
                            </td>
                        </tr>
                        <tr style="display:none;">
                            <td align="left" colspan="4">
                                <asp:CheckBox ID="Chk_Verificar" runat="server" Text="Verificar características de productos"
                                    ToolTip="Verificar las características, garantía y pólizas de mantenimiento de la mercancía cuando se reciba del proveedor" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                *Justificación de la compra
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="4">
                                <asp:TextBox ID="Txt_Justificacion" runat="server" MaxLength="3000" TabIndex="10"
                                    TextMode="MultiLine" Width="100%" Height="105px"></asp:TextBox>
                                <cc1:textboxwatermarkextender id="TextBoxWatermarkExtender3" runat="server" targetcontrolid="Txt_Justificacion"
                                    watermarktext="&lt;Límite de Caracteres 3000&gt;" watermarkcssclass="watermarked">
                                    </cc1:textboxwatermarkextender>
                                <cc1:filteredtextboxextender id="FTE_Justificacion" runat="server" targetcontrolid="Txt_Justificacion"
                                    filtertype="Custom, UppercaseLetters, LowercaseLetters, Numbers" validchars="ÑñáéíóúÁÉÍÓÚ./*-!$%&()=,[]{}+<>@?¡?¿# ">
                                    </cc1:filteredtextboxextender>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="Lbl_Especificaciones_Adicionales" runat="server" Text="Especificaciones adicionales"
                                    Visible="false"></asp:Label>
                            </td>
                            <td align="left" colspan="3">
                                <asp:TextBox ID="Txt_Especificaciones" runat="server" MaxLength="3000" TabIndex="10"
                                    TextMode="MultiLine" Width="100%" Height="35px" Visible="false"></asp:TextBox>
                                <cc1:textboxwatermarkextender id="TextBoxWatermarkExtender2" runat="server" targetcontrolid="Txt_Especificaciones"
                                    watermarktext="&lt;Límite de Caracteres 3000&gt;" watermarkcssclass="watermarked">
                                    </cc1:textboxwatermarkextender>
                                <cc1:filteredtextboxextender id="FTE_Especificaciones" runat="server" targetcontrolid="Txt_Especificaciones"
                                    filtertype="Custom, UppercaseLetters, LowercaseLetters, Numbers" validchars="ÑñáéíóúÁÉÍÓÚ./*-!$%&()=,[]{}+<>@?¡?¿# ">
                                    </cc1:filteredtextboxextender>
                            </td>
                        </tr>
                        <div id="Div_Comentarios" runat="server">
                            <tr>
                                <td align="left" colspan="4">
                                    Comentarios
                                    <asp:LinkButton ID="Lnk_Observaciones" runat="server" OnClick="Lnk_Observaciones_Click"
                                        Visible="false">Mostrar</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="4">
                                    <asp:TextBox ID="Txt_Comentario" runat="server" MaxLength="1500" TabIndex="10" TextMode="MultiLine"
                                        Width="100%" Height="35px"></asp:TextBox>
                                    <cc1:textboxwatermarkextender id="Txt_Comentario_TextBoxWatermarkExtender" runat="server"
                                        targetcontrolid="Txt_Comentario" watermarktext="&lt;Límite de Caracteres 1500&gt;"
                                        watermarkcssclass="watermarked">
                                    </cc1:textboxwatermarkextender>
                                    <cc1:filteredtextboxextender id="FTE_Comentario" runat="server" targetcontrolid="Txt_Comentario"
                                        filtertype="Custom, UppercaseLetters, LowercaseLetters, Numbers" validchars="ÑñáéíóúÁÉÍÓÚ. ">
                                    </cc1:filteredtextboxextender>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top" colspan="4">
                                    <asp:GridView ID="Grid_Comentarios" runat="server" AllowPaging="true" AutoGenerateColumns="false"
                                        GridLines="None" PageSize="3" Width="100%" OnSelectedIndexChanged="Grid_Comentarios_SelectedIndexChanged"
                                        Style="font-size: xx-small; white-space: normal" OnPageIndexChanging="Grid_Comentarios_PageIndexChanging">
                                        <RowStyle CssClass="GridItem" />
                                        <Columns>
                                            <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png"
                                                Visible="false">
                                                <ItemStyle Width="5%" />
                                            </asp:ButtonField>
                                            <asp:BoundField DataField="Comentario" HeaderText="Comentarios" Visible="True">
                                                <HeaderStyle HorizontalAlign="Left" Width="68%" />
                                                <ItemStyle HorizontalAlign="Left" Width="68%" Font-Size="X-Small" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Estatus" HeaderText="Estatus" Visible="false">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Fecha_Creo" HeaderText="Fecha" Visible="True">
                                                <HeaderStyle HorizontalAlign="Left" Width="8%" />
                                                <ItemStyle HorizontalAlign="Left" Width="8%" Font-Size="X-Small" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Usuario_Creo" HeaderText="Usuario" Visible="True">
                                                <HeaderStyle HorizontalAlign="Left" Width="24%" />
                                                <ItemStyle HorizontalAlign="Left" Width="24%" Font-Size="X-Small" />
                                            </asp:BoundField>
                                        </Columns>
                                        <PagerStyle CssClass="GridHeader" />
                                        <SelectedRowStyle CssClass="GridSelected" />
                                        <HeaderStyle CssClass="GridHeader" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                    </asp:GridView>
                                </td>
                            </tr>
                        </div>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UPnl_Busqueda" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Button ID="Button" runat="server" Text="Button" Style="display: none;" />
                <cc1:modalpopupextender id="Modal_Busqueda_Prod_Serv" runat="server" targetcontrolid="Btn_Comodin_Busqueda_Productos_Srv"
                    popupcontrolid="Modal_Productos_Servicios" cancelcontrolid="Btn_Cerrar" dynamicservicepath=""
                    dropshadow="True" backgroundcssclass="progressBackgroundFilter" />
                <%--<asp:Button ID="Btn_Comodin_1" runat="server" Text="Button" style="display:none;" /> --%>
                <asp:Button ID="Btn_Comodin_Busqueda_Productos_Srv" runat="server" Text="Button"
                    Style="display: none" />
                <%--<asp:Button ID="Btn_Comodin_Close" runat="server" Text="Button" style="display:none;" />--%>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Panel ID="Modal_Productos_Servicios" runat="server" CssClass="drag" HorizontalAlign="Center"
            Style="display: none; border-style: outset; border-color: Silver; width: 860px;">
            <asp:Panel ID="Panel2" runat="server" CssClass="estilo_fuente" Style="cursor: move;
                background-color: Silver; color: Black; font-size: 12; font-weight: bold; border-style: outset;">
                <table class="estilo_fuente">
                    <tr>
                        <td style="color: Black; font-size: 12; font-weight: bold;">
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                            Buscar Producto
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <center>
                <asp:UpdatePanel ID="MP_UDPpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div>
                            <table width="100%" style="color: Black;">
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td style="width: 10%;">
                                        Nombre
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txt_Nombre" runat="server" Width="98.5%" OnTextChanged="Txt_Nombre_TextChanged"
                                            AutoPostBack="true">
                                        </asp:TextBox>
                                    </td>
                                    <td style="width: 10%;">
                                        <asp:ImageButton ID="IBtn_MDP_Prod_Serv_Buscar" runat="server" CssClass="Img_Button"
                                            OnClick="IBtn_MDP_Prod_Serv_Buscar_Click" ToolTip="Búscar producto ó servicio"
                                            ImageUrl="~/paginas/imagenes/paginas/busqueda.png" />
                                        <asp:ImageButton ID="IBtn_MDP_Prod_Serv_Cerrar" runat="server" CssClass="Img_Button"
                                            OnClick="IBtn_MDP_Prod_Serv_Cerrar_Click" ToolTip="Búscar producto ó servicio"
                                            ImageUrl="~/paginas/imagenes/paginas/quitar.png" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                            <div style="overflow: auto; height: 260px; width: 99%; vertical-align: top; border-style: outset;
                                border-color: Silver;">
                                <table width="100%" style="color: Black;">
                                    <tr>
                                        <td align="center">
                                            <asp:GridView ID="Grid_Productos_Servicios_Modal" runat="server" AllowPaging="false"
                                                AutoGenerateColumns="false" CssClass="GridView_1" DataKeyNames="ID,NOMBRE,DISPONIBLE,COSTO"
                                                GridLines="Vertical" PageSize="10" Width="98%" OnPageIndexChanging="Grid_Productos_Servicios_Modal_PageIndexChanging"
                                                Style="white-space: normal" EmptyDataText="No se encontraron registros">
                                                <RowStyle CssClass="GridItem" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="Btn_Seleccionar_Producto" runat="server" ImageUrl="~/paginas/imagenes/gridview/blue_button.png"
                                                                OnClick="Btn_Seleccionar_Producto_Click" CommandArgument='<%# Eval("ID") %>' />
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" Width="3%" />
                                                        <ItemStyle HorizontalAlign="Center" Width="3%" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="CLAVE" HeaderText="Clave" Visible="True">
                                                        <FooterStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Width="7%" Font-Size="X-Small" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" Visible="True">
                                                        <FooterStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Wrap="true" Width="12%" Font-Size="X-Small" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripción" Visible="True">
                                                        <FooterStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Wrap="true" Font-Size="X-Small" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="MODELO" HeaderText="Modelo" Visible="True">
                                                        <FooterStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Wrap="true" Width="10%" Font-Size="X-Small" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="UNIDAD" HeaderText="Uni." Visible="True">
                                                        <FooterStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Wrap="true" Width="7%" Font-Size="X-Small" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="COSTO" HeaderText="P. Unitario" Visible="True" DataFormatString="{0:C}">
                                                        <FooterStyle HorizontalAlign="Right" />
                                                        <HeaderStyle HorizontalAlign="Right" />
                                                        <ItemStyle HorizontalAlign="Right" Width="10%" Font-Size="X-Small" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DISPONIBLE" HeaderText="Disp." Visible="True">
                                                        <FooterStyle HorizontalAlign="Right" />
                                                        <HeaderStyle HorizontalAlign="Right" />
                                                        <ItemStyle HorizontalAlign="Center" Width="8%" Font-Size="X-Small" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ID" HeaderText="Producto_Servicio_ID" Visible="False">
                                                        <FooterStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <PagerStyle CssClass="GridHeader" />
                                                <SelectedRowStyle CssClass="GridSelected" />
                                                <HeaderStyle CssClass="GridHeader" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </center>
        </asp:Panel>
        <%-- Panel del ModalPopUp--%>
    </div>
    <asp:Panel ID="Modal_Productos_ServiciosX" runat="server" CssClass="drag" HorizontalAlign="Center"
        Style="display: none; border-style: outset; border-color: Silver; width: 760px;">
        <asp:Button ID="Btn_Realizar_Busqueda" runat="server" Text="Buscar" CssClass="button" />
        <asp:Button ID="Btn_Cerrar" runat="server" Text="Cerrar" CssClass="button" />
    </asp:Panel>
</asp:Content>

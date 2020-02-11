<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" CodeFile="Frm_Ope_Cat_Asignacion_Avaluos_Urbanos.aspx.cs" Inherits="paginas_Catastro_Frm_Ope_Cat_Asignacion_Avaluos_Urbanos" Title="Asignación de Avalúos Urbanos"%>

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

    function Abrir_Busqueda_Peritos_Internos() {
        $find('Busqueda_Peritos_Internos').show();
        return false;
    }

    function Abrir_Busqueda_Avaluos() {
        $find('Busqueda_Avaluos').show();
        return false;
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
                            Asignación de Avalúos
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
                                        <asp:ImageButton ID="Btn_Nuevo" runat="server" TabIndex="1" 
                                ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" Width="24px" CssClass="Img_Button" 
                                AlternateText="Nuevo" OnClick = "Btn_Nuevo_Click"/>
                                            <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button"
                                                TabIndex="2" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" AlternateText="Modificar"
                                                OnClick="Btn_Modificar_Click" />
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                                TabIndex="5" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" AlternateText="Salir"
                                                OnClick="Btn_Salir_Click" />
                                        </td>
                                        <td align="right" style="width: 41%;">
                                            <table style="width: 100%; height: 28px;">
                                                <tr>
                                                    <td style="vertical-align: middle; text-align: right; width: 20%;">
                                                        B&uacute;squeda:
                                                    </td>
                                                    <td style="width: 55%;">
                                                        <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100" TabIndex="5" ToolTip="Buscar"
                                                            Width="180px" Style="text-transform: uppercase" />
                                                        <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                                                            WatermarkText="<Buscar>" TargetControlID="Txt_Busqueda" />
                                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" runat="server" TargetControlID="Txt_Busqueda"
                                                            FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. " />
                                                    </td>
                                                    <td style="vertical-align: middle; width: 5%;">
                                                        <asp:ImageButton ID="Btn_Buscar" runat="server" TabIndex="6" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                                            ToolTip="Buscar" AlternateText="Buscar" OnClick="Btn_Buscar_Click" />
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
                    <td style="text-align: left; width: 20%;">
                            Perito Interno
                        </td>
                        <td style="width: 30%; text-align: left;">
                            <asp:TextBox runat="server" ID="Txt_Perito_Interno" Width="92%"
                                Style="float: left; text-transform: uppercase" MaxLength="50" Enabled="false"/>
                                <asp:ImageButton ID="Btn_Mostrar_Busqueda_Avanzada_Peritos_Internos" runat="server" 
                                Height="24px" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" 
                                onclientclick="javascript:return Abrir_Busqueda_Peritos_Internos();" TabIndex="10" 
                                ToolTip="Búsqueda Avanzada de Tasas" Width="24px" />
                                <asp:HiddenField ID="Hdf_Perito_Interno_Id" runat="server" />
                            <asp:HiddenField ID="Hdf_Empleado_Id" runat="server" />
                        </td>
                        <td style="text-align: left; width: 20%;">
                            Avalúo
                        </td>
                        <td style="width: 30%; text-align: left;">
                            <asp:TextBox runat="server" ID="Txt_Avaluo" Width="92%"
                                Style="float: left; text-transform: uppercase" MaxLength="50" Enabled="false"/>
                                <asp:ImageButton ID="Btn_Mostrar_Busqueda_Avanzada_Tasas" runat="server" 
                                Height="24px" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" 
                                onclientclick="javascript:return Abrir_Busqueda_Avaluos();" TabIndex="10" 
                                ToolTip="Búsqueda Avanzada de Tasas" Width="24px" />
                            <asp:HiddenField ID="Hdf_No_Avaluo" runat="server" />
                            <asp:HiddenField ID="Hdf_Anio_Avaluo" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <table width="98%" class="estilo_fuente">
                                <tr>
                                    <td style="text-align: left;">
                                        <asp:GridView ID="Grid_Avaluos_Asignados" runat="server" AllowPaging="True" AllowSorting="True" 
                                                AutoGenerateColumns="False" CssClass="GridView_1" 
                                                EmptyDataText="&quot;No se encontraron registros&quot;"
                                                HeaderStyle-CssClass="tblHead" PageSize="15" style="white-space:normal;" 
                                                Width="100%"
                                                OnSelectedIndexChanged = "Grid_Avaluos_Asignados_SelectedIndexChanged"
                                                OnPageIndexChanging = "Grid_Avaluos_Asignados_PageIndexChanging">
                                                <Columns>
                                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                        <ItemStyle HorizontalAlign="Center" Width="3.5%" />
                                                    
                                                    </asp:ButtonField>
                                                    <asp:BoundField DataField="NO_AVALUO" HeaderStyle-Width="15%" HeaderText="ID" Visible="false">
                                                    <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                    </asp:BoundField>
                                                    
                                                    <asp:BoundField DataField="ANIO_AVALUO" HeaderStyle-Width="15%" HeaderText="Anio" Visible="false">
                                                    <HeaderStyle HorizontalAlign="Left" Width="70%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="70%" />
                                                    </asp:BoundField>
                                                    
                                                    <asp:BoundField DataField="AVALUO" HeaderText="Avaluo">
                                                    <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Center" Width="20%" />
                                                    </asp:BoundField>

                                                    <asp:BoundField DataField="PERITO_INTERNO_ID" HeaderText="Perito Interno id" Visible="false">
                                                    <HeaderStyle HorizontalAlign="Center" Width="70%" />
                                                    <ItemStyle HorizontalAlign="Center" Width="70%" />
                                                    </asp:BoundField>

                                                    <asp:BoundField DataField="PERITO_INTERNO" HeaderText="Perito Interno Asignado">
                                                    <HeaderStyle HorizontalAlign="Left" Width="60%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="60%" />
                                                    </asp:BoundField>

                                                    <asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                                    <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Center" Width="20%" />
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
        </ContentTemplate>
    </asp:UpdatePanel>


    <cc1:ModalPopupExtender ID="Mpe_Busqueda_Peritos_Internos" runat="server" 
                    TargetControlID="Btn_Comodin_Open_Busqueda_Peritos_Internos"
                    PopupControlID="Pnl_Busqueda_Contenedor" 
                    BackgroundCssClass="popUpStyle"
                    BehaviorID="Busqueda_Peritos_Internos"
                    CancelControlID="Btn_Comodin_Close_Busqueda_Peritos_Internos" 
                    DropShadow="true" 
                    DynamicServicePath="" 
                    Enabled="True" />
                <asp:Button ID="Btn_Comodin_Close_Busqueda_Peritos_Internos" runat="server"
                    Style="background-color: transparent; border-style:none;display:none;"  Text="" />
                <asp:Button  ID="Btn_Comodin_Open_Busqueda_Peritos_Internos" runat="server"
                    Style="background-color:transparent; border-style:none;display:none;" Text="" />

    <%--Ventana modal de búsqueda de peritos internos--%>
    <asp:Panel ID="Pnl_Busqueda_Contenedor" runat="server" CssClass="drag"  HorizontalAlign="Center" Width="650px" 
    style="display:none;border-style:outset;border-color:Silver;background-image:url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG);background-repeat:repeat-y;">                         
    <asp:Panel ID="Panel1" runat="server" 
        style="cursor: move;background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
        <table width="99%">
            <tr>
                <td style="color:Black;font-size:12;font-weight:bold;">
                   <asp:Image ID="Img_Informatcion_Autorizacion" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                     B&uacute;squeda: Peritos Internos
                </td>
                <td align="right" style="width:10%;">
                   <asp:ImageButton ID="Btn_Cerrar_Ventana" CausesValidation="false" runat="server" style="cursor:pointer;" ToolTip="Cerrar Ventana"
                        ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png" OnClick="Btn_Cerrar_Ventana_Click"/>  
                </td>
            </tr>
        </table>            
    </asp:Panel>                                                                          
                        <asp:UpdatePanel ID="Upnl_Filtros_Busqueda_Prestamos" runat="server">
                            <ContentTemplate>
                            <div style="color: #5D7B9D">
                                <table width="100%">
                                    <tr>
                                    <td align="left" style="text-align: left;" >
                                <asp:UpdateProgress ID="Progress_Upnl_Filtros_Busqueda_Prestamos" runat="server" AssociatedUpdatePanelID="Upnl_Filtros_Busqueda_Prestamos" DisplayAfter="0">
                                    <ProgressTemplate>
                                        <div id="progressBackgroundFilter1" class="progressBackgroundFilter"></div>
                                        <div  style="background-color:Transparent;position:fixed;top:50%;left:47%;padding:10px; z-index:1002;" id="div_progress1"><img alt="" src="../Imagenes/paginas/Sias_Roler.gif" /></div>
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
                                        <td style="width:100%" colspan="4">
                                            <hr />
                                        </td>
                                    </tr>   
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                          Nombre Perito Interno
                                        </td>              
                                        <td style="width:30%;text-align:right;font-size:11px;" colspan="3">
                                           <asp:TextBox ID="Txt_Busqueda_Nombre" runat="server" Width="98%"/>
                                        </td> 
                                    </tr>
                                    <tr>
                                    <td colspan="4">
                                <asp:GridView ID="Grid_Peritos_Externos" runat="server" AllowPaging="True" AllowSorting="True" 
                                                AutoGenerateColumns="False" CssClass="GridView_1" 
                                                EmptyDataText="&quot;No se encontraron registros&quot;"
                                                HeaderStyle-CssClass="tblHead" PageSize="15" style="white-space:normal;" 
                                                Width="100%"
                                                OnSelectedIndexChanged = "Grid_Peritos_Externos_SelectedIndexChanged"
                                                OnPageIndexChanging = "Grid_Peritos_Externos_PageIndexChanging">
                                                <Columns>
                                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                        <ItemStyle HorizontalAlign="Center" Width="3.5%" />
                                                    </asp:ButtonField>

                                                    <asp:BoundField DataField="PERITO_INTERNO_ID" HeaderStyle-Width="15%" HeaderText="ID" Visible="false">
                                                    <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                    </asp:BoundField>

                                                    <asp:BoundField DataField="EMPLEADO_ID" HeaderStyle-Width="15%" HeaderText="ID_Empleado" Visible="false">
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
                                        <td style="width:100%;text-align:left;" colspan="4">
                                            <center>
                                                 <asp:Button ID="Btn_Busqueda_Empleados" runat="server"  Text="Busqueda Peritos Internos" CssClass="button"  
                                                CausesValidation="false" Width="200px" OnClick="Btn_Busqueda_Empleados_Click"/>
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

    <cc1:ModalPopupExtender ID="Mpe_Avaluos" runat="server" 
                    TargetControlID="Btn_Comodin_Open_Avaluo"
                    PopupControlID="Pnl_Avaluos" 
                    BackgroundCssClass="popUpStyle"
                    BehaviorID="Busqueda_Avaluos"
                    CancelControlID="Btn_Comodin_Close_Avaluo" 
                    DropShadow="true" 
                    DynamicServicePath="" 
                    Enabled="True" />
                <asp:Button ID="Btn_Comodin_Open_Avaluo" runat="server"
                    Style="background-color: transparent; border-style:none;display:none;"  Text="" />
                <asp:Button  ID="Btn_Comodin_Close_Avaluo" runat="server"
                    Style="background-color:transparent; border-style:none;display:none;" Text="" />

    <%--Ventana modal de búsqueda de peritos internos--%>
    <asp:Panel ID="Pnl_Avaluos" runat="server" CssClass="drag"  HorizontalAlign="Center" Width="650px" 
    style="display:none;border-style:outset;border-color:Silver;background-image:url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG);background-repeat:repeat-y;">                         
    <asp:Panel ID="Pnl_Titulo_Avaluos" runat="server" 
        style="cursor: move;background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
        <table width="99%">
            <tr>
                <td style="color:Black;font-size:12;font-weight:bold;">
                   <asp:Image ID="Image1" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                     B&uacute;squeda: Avalúos Urbano
                </td>
                <td align="right" style="width:10%;">
                   <asp:ImageButton ID="Btn_Cerrar_Ventana_Avaluo" CausesValidation="false" runat="server" style="cursor:pointer;" ToolTip="Cerrar Ventana"
                        ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png" OnClick="Btn_Cerrar_Ventana_Avaluo_Click"/>  
                </td>
            </tr>
        </table>            
    </asp:Panel>                                                                          
                        <asp:UpdatePanel ID="Udp_Avaluos" runat="server">
                            <ContentTemplate>
                            <div style="color: #5D7B9D">
                                <table width="100%">
                                    <tr>
                                    <td align="left" style="text-align: left;" >
                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Udp_Avaluos" DisplayAfter="0">
                                    <ProgressTemplate>
                                        <div id="progressBackgroundFilter1" class="progressBackgroundFilter"></div>
                                        <div  style="background-color:Transparent;position:fixed;top:50%;left:47%;padding:10px; z-index:1002;" id="div_progress1"><img alt="" src="../Imagenes/paginas/Sias_Roler.gif" /></div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress> 
                                                             
                                  <table width="100%">
<%--                                   <tr>
                                        <td style="width:100%" colspan="4" align="right">
                                            <asp:ImageButton ID="Btn_Limpiar_Avaluo" runat="server" OnClientClick="javascript:return Limpiar_Ctlr();"
                                                ImageUrl="~/paginas/imagenes/paginas/sias_clear.png" ToolTip="Limpiar Controles de Busqueda"/>                         
                                        </td>
                                    </tr>     --%>
                                   <tr>
                                        <td style="width:100%" colspan="4">
                                            <hr />
                                        </td>
                                    </tr>   
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                          Folio Avalúo
                                        </td>              
                                        <td style="width:30%;text-align:right;font-size:11px;">
                                           <asp:TextBox ID="Txt_Busqueda_Avaluo" runat="server" Width="98%"/>
                                        </td> 
                                    </tr>
                                    <tr>
                                    <td colspan="4">
                                <asp:GridView ID="Grid_Avaluos_Urbanos" runat="server" AllowPaging="True" AllowSorting="True" 
                                                AutoGenerateColumns="False" CssClass="GridView_1" 
                                                EmptyDataText="&quot;No se encontraron registros&quot;"
                                                HeaderStyle-CssClass="tblHead" PageSize="15" style="white-space:normal;" 
                                                Width="100%"
                                                OnSelectedIndexChanged = "Grid_Avaluos_Urbanos_SelectedIndexChanged"
                                                OnPageIndexChanging = "Grid_Avaluos_Urbanos_PageIndexChanging">
                                                <Columns>
                                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                        <ItemStyle HorizontalAlign="Center" Width="3.5%" />
                                                    
                                                    </asp:ButtonField>
                                                    <asp:BoundField DataField="NO_AVALUO" HeaderStyle-Width="15%" HeaderText="ID" Visible="false">
                                                    <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                    </asp:BoundField>
                                                    
                                                    <asp:BoundField DataField="ANIO_AVALUO" HeaderStyle-Width="15%" HeaderText="Anio" Visible="false">
                                                    <HeaderStyle HorizontalAlign="Left" Width="70%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="70%" />
                                                    </asp:BoundField>
                                                    
                                                    <asp:BoundField DataField="AVALUO" HeaderText="Avaluo">
                                                    <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Center" Width="20%" />
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
                                        <td style="width:100%;text-align:left;" colspan="4">
                                            <center>
                                                 <asp:Button ID="Btn_Busqueda_Avaluos" runat="server"  Text="Busqueda de Avalúos" CssClass="button"  
                                                CausesValidation="false" Width="200px" OnClick="Btn_Busqueda_Avaluos_Click"/>
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
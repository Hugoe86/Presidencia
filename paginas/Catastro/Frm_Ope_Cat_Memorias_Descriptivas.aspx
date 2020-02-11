<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Ope_Cat_Memorias_Descriptivas.aspx.cs"
    Inherits="paginas_Catastro_Frm_Ope_Cat_Memorias_Descriptivas" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" %>

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
            padding: 0 5px 0 5px;
        }
        a.enlace_fotografia:hover
        {
            color: #25406D;
            text-decoration: underline;
            font-weight: bold;
            padding: 0 5px 0 5px;
        }
        .style1
        {
            width: 239px;
        }
        .style2
        {
            width: 39%;
        }
        .style3
        {
            width: 20%;
            height: 15px;
        }
        .style4
        {
            width: 30%;
            height: 15px;
        }
    </style>
    <!--SCRIPT PARA LA VALIDACION QUE NO EXPIRE LA SESSION-->

    <script type="text/javascript" language="javascript">
        //El nombre del controlador que mantiene la sesión
        var CONTROLADOR = "../../Mantenedor_sesiones.ashx";

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
        //        setInterval('MantenSesion()', <%=(int)(0.9*(Session.Timeout * 60000))%>);

        window.onerror = new Function("return true");
        //Abrir una ventana modal
        function Abrir_Ventana_Modal(Url, Propiedades) {
            window.showModalDialog(Url, null, Propiedades);
        }

        function Abrir_Resumen(Url, Propiedades) {
            window.open(Url, 'Resumen_Predio', Propiedades);
        }
        function Abrir_Busqueda_Peritos_Externo() {
        $find('Busqueda_Peritos_Externos').show();
        return false;
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
                        <td class="label_titulo">
                            Memorias Descriptivas
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
                                            <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button"
                                                TabIndex="1" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" OnClick="Btn_Nuevo_Click"
                                                AlternateText="Nuevo" />
                                            <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" OnClick="Btn_Modificar_Click"
                                                AlternateText="Modificar" />
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Salir_Click"
                                                AlternateText="Salir" TabIndex="2" />
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
                <div id="Div_Grid_Datos_Peritos" runat="server" visible="true">
                    <table width="98%" border="0" cellspacing="0" class="estilo_fuente">
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                <asp:HiddenField ID="Hdf_No_Mem_Descript" runat="server" />
                                <asp:HiddenField ID="Hdf_Anio" runat="server" />
                                <asp:HiddenField ID="Hdf_Perito_Externo_Id" runat="server" />
                                <asp:HiddenField ID="Hdf_Cantidad_Cobro1" runat="server" />
                                <asp:HiddenField ID="Hdf_Cantidad_Cobro2" runat="server" />
                                <asp:HiddenField ID="Hdf_Cuenta_Predial_Id" runat="server" />
                                <asp:HiddenField ID="Hdf_Adeudo_Anterior" runat="server" />
                            </td>
                        </tr>
                         <tr style="background-color: #3366CC">
                    <td style="text-align: left; font-size: 15px; color: #FFFFFF;" colspan="4">
                            Datos del Perito Externo
                        </td>
                        
                    </tr>
                        
                         <tr>
                    <td style="text-align: left; width: 20%;">
                            Perito Externo
                        </td>
                        <td style="width: 30%; text-align: left;">
                            <asp:TextBox runat="server" ID="Txt_Perito_Externo" Width="92%"
                                Style="float: left; text-transform: uppercase" MaxLength="50" Enabled="false"/>
                                <asp:ImageButton ID="Btn_Mostrar_Busqueda_Avanzada_Peritos_Externo" runat="server" 
                                Height="24px" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" 
                                onclientclick="javascript:return Abrir_Busqueda_Peritos_Externo();" TabIndex="10" 
                                ToolTip="Búsqueda Avanzada de Peritos Externos" Width="24px" 
                                 />
                                
                               
                        </td>
                        
                    </tr>
                        
                        <tr style="background-color: #3366CC">
                            <td style="text-align: left; font-size: 15px; color: #FFFFFF;" colspan="4">
                                Datos del Tramite
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%">
                                *Fraccionamiento
                            </td>
                            <td style="width: 30%">
                                <asp:TextBox ID="Txt_Fraccionamiento" runat="server" Width="98%" TabIndex="3" MaxLength="50"
                                    Style="text-transform:uppercase"></asp:TextBox>
                            </td>
                            <td style="width: 20%">
                                *Estatus
                            </td>
                            <td style="width: 30%">
                                <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="98%" Enabled="false">
                                    <asp:ListItem Text="VIGENTE" Value="VIGENTE" />
                                    <asp:ListItem Text="BAJA" Value="BAJA" />
                                    <asp:ListItem Text="AUTORIZADA" Value="AUTORIZADA" />
                                    <asp:ListItem Text="RECHAZADA" Value="RECHAZADA" />
                                    <asp:ListItem Text="PAGADA" Value="PAGADA" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="style3">
                                *Propietario/Solicitante
                            </td>
                            <td class="style4">
                                <asp:TextBox ID="Txt_Solicitante" runat="server" Width="98%" TabIndex="9" MaxLength="50"
                                    Style="text-transform: uppercase"></asp:TextBox>
                            </td>
                            <td style="text-align: left; " class="style3">
                                *Tipo
                            </td>
                            <td style="text-align: left; " class="style4">
                                <asp:DropDownList ID="Cmb_Tipo" runat="server" Width="98%" AutoPostBack="true" OnSelectedIndexChanged="Cmb_Tipo_SelectedIndexChanged">
                                    <asp:ListItem Text="<SELECCIONE>" Value="SELECCCIONE" />
                                    <asp:ListItem Text="RC" Value="RC" />
                                    <asp:ListItem Text="MR" Value="MR" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                        <td style="text-align: left; width: 20%;">
                                *Cuenta Predial
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Cuent_Predial" runat="server" Width="98%" TabIndex="3" Style="text-transform: uppercase"
                                    Enabled="false"  MaxLength="12" AutoPostBack="true" OnTextChanged="Txt_Cuenta_Predial_TextChanged" ></asp:TextBox>
                                
                            </td>
                            <td style="text-align: left; " class="style3">
                                *Horientación
                            </td>
                            <td style="text-align: left; " class="style4">
                                <asp:DropDownList ID="Cmb_Horientacion" runat="server" Width="98%" AutoPostBack="true" >
                                    <asp:ListItem Text="<SELECCIONE>" Value="SELECCCIONE" />
                                    <asp:ListItem Text="HORIZONTAL" Value="HORIZONTAL" />
                                    <asp:ListItem Text="VERTICAL" Value="VERTICAL" />
                                    
                                </asp:DropDownList>
                            </td>
                        
                        
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                *No. Memorias Descriptivas
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_No_Memorias_Descriptivas" runat="server" Width="98%" TabIndex="3"
                                    Enabled="false" MaxLength="4" AutoPostBack="true" OnTextChanged="Txt_No_Memorias_Descriptivas_TextChanged" ></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Ftb_Txt_No_Memorias_Descriptivas" runat="server"
                                    FilterType="Numbers,Custom" TargetControlID="Txt_No_Memorias_Descriptivas"  />
                            </td>
                        </tr>
                        <tr>
                        <td style="text-align: left; width: 20%;">
                                *Ubicacion
                            </td>
                            <td style="text-align: left; width: 30%;" colspan="3">
                                <asp:TextBox ID="Txt_Ubicacion" runat="server" Width="98%" TabIndex="3" Style="text-transform: uppercase"
                                    Enabled="false" MaxLength="100" AutoPostBack="true"  ></asp:TextBox>
                                
                            </td>
                        </tr>
                        <tr style="background-color: #36C;">
                            <td colspan="4" style="text-align: left; font-size: 15px; color: #FFF;">
                                Observaciones
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                *Observaciones
                            </td>
                            <td style="text-align: left; width: 30%;" colspan="3">
                              
                                <asp:TextBox ID="Txt_Observacion" runat="server" Enabled="false" Height="77px" MaxLength="250"
                                    TabIndex="3" Width="97%" ReadOnly="true" Style="text-transform:uppercase"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="Div_Detalles" runat="server" visible="true">
                    <table width="98%" class="estilo_fuente">
                        <tr style="background-color: #36C;">
                            <td colspan="4" style="text-align: left; font-size: 15px; color: #FFF;">
                                Documentos
                            </td>
                        </tr>
                        <tr id="Tr_Fila_Fotografias_Bien" runat="server">
                            <td style="text-align: left; width: 20%" class="style1">
                                Nombre del Documento
                            </td>
                            <td style="text-align: left; width: 30%" class="style1">
                                <asp:DropDownList ID="Cmb_Documento" runat="server" Width="98%" />
                            </td>
                            <td style="text-align: left; width: 20%" class="style2">
                                Documento
                            </td>
                            <td style="width: 30%" class="style2">
                                <asp:FileUpload ID="Fup_Documento" runat="server" Width="98%" TabIndex="16" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%">
                            </td>
                            <td style="width: 30%">
                            </td>
                            <td style="width: 20%">
                            </td>
                            <td style="width: 30%">
                                <asp:ImageButton ID="Btn_Agregar_Documento" runat="server" Height="20px" ImageUrl="~/paginas/imagenes/paginas/sias_add.png"
                                    OnClick="Btn_Agregar_Documento_Click" ToolTip="Agregar Documento" Width="20px"
                                    TabIndex="17" />
                            </td>
                           
                        </tr>
                         <tr>
                            <td style="text-align: left; width: 20%" class="style2">
                                Importe del Tramite
                            </td>
                            <td style="width: 30%" class="style2">
                                <asp:TextBox ID="Txt_Calculo_Valores_Memorias" runat="server" Width="98%" TabIndex="3"
                                    Enabled="false" MaxLength="20" AutoPostBack="true" OnTextChanged="Txt_Calculo_Valores_Memorias_TextChanged"></asp:TextBox>
                            </td>
                            <td style="text-align: left; width: 20%" class="style2">
                                No. Recibo
                            </td>
                            <td style="width: 30%" class="style2">
                                <asp:TextBox ID="Txt_No_Recibo" runat="server" Width="98%" Enabled="false" ></asp:TextBox>
                            </td>
                            </tr>
                        <tr>
                            <td colspan="4" style="text-align: left; width: 20%;">
                                <asp:GridView ID="Grid_Documentos" runat="server" AllowPaging="False" AllowSorting="True"
                                    AutoGenerateColumns="False" HeaderStyle-CssClass="tblHead" OnSelectedIndexChanged="Grid_Documentos_SelectedIndexChanged"
                                    PageSize="20" OnDataBound="Grid_Documentos_DataBound" Style="white-space: normal;"
                                    Width="100%">
                                    <Columns>
                                        <asp:BoundField DataField="NO_DOCUMENTO" HeaderText="No documento" Visible="false">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ANIO_DOCUMENTO" HeaderText="Año documento" Visible="false">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="REGIMEN_CONDOMINIO_ID" HeaderText="Perito Id" Visible="false">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DOCUMENTO" HeaderText="Nombre Documento">
                                            <ItemStyle HorizontalAlign="Left" Width="40%" />
                                        </asp:BoundField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                            </ItemTemplate>
                                            <HeaderStyle Width="50%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="Btn_Eliminar_Fotos" runat="server" CommandName="Select" Height="20px"
                                                    ImageUrl="~/paginas/imagenes/paginas/delete.png" ToolTip="Eliminar" Width="20px" />
                                            </ItemTemplate>
                                            <HeaderStyle Width="2%" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="tblHead" />
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:GridView ID="Grid_Memorias_Descriptivas" runat="server" AllowPaging="True" AllowSorting="True"
                                    AutoGenerateColumns="False" CssClass="GridView_1" EmptyDataText="&quot;No se encontraron registros&quot;"
                                    HeaderStyle-CssClass="tblHead" PageSize="15" Style="white-space: normal;" Width="100%"
                                    OnSelectedIndexChanged="Grid_Memorias_Descriptivas_SelectedIndexChanged" OnPageIndexChanging="Grid_Memorias_Descriptivas_PageIndexChanging">
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                            <ItemStyle HorizontalAlign="Center" Width="3.5%" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="NO_MEM_DESCRIPT" HeaderStyle-Width="15%" HeaderText="ID"
                                            Visible="false">
                                            <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                            <ItemStyle HorizontalAlign="Left" Width="0%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ANIO" HeaderStyle-Width="15%" HeaderText="Año" Visible="false">
                                            <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                            <ItemStyle HorizontalAlign="Left" Width="0%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CANTIDAD_MEM_DESCRIPT" HeaderStyle-Width="30%" HeaderText="Cantidad de Memorias Descriptivas">
                                            <HeaderStyle HorizontalAlign="Center" Width="30%" />
                                            <ItemStyle HorizontalAlign="Center" Width="30%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TIPO" HeaderStyle-Width="30%" HeaderText="Tipo de Trámite">
                                            <HeaderStyle HorizontalAlign="Center" Width="30%" />
                                            <ItemStyle HorizontalAlign="Center" Width="30%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" HeaderStyle-Width="40%">
                                            <HeaderStyle HorizontalAlign="Center" Width="40%" />
                                            <ItemStyle HorizontalAlign="Center" Width="40%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="OBSERVACIONES" HeaderText="Observaciones" Visible="false">
                                            <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                            <ItemStyle HorizontalAlign="Center" Width="20%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FRACCIONAMIENTO" HeaderText="Fraccionamiento" Visible="false">
                                            <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                            <ItemStyle HorizontalAlign="Center" Width="20%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SOLICITANTE" HeaderText="Solicitante" Visible="false">
                                            <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                            <ItemStyle HorizontalAlign="Center" Width="20%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="UBICACION" HeaderText="Ubicacion" Visible="false">
                                            <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                            <ItemStyle HorizontalAlign="Center" Width="20%" />
                                        </asp:BoundField>
                                         <asp:BoundField DataField="CUENTA_PREDIAL_ID" HeaderText="Cuenta Predial Id" Visible="false">
                                            <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                            <ItemStyle HorizontalAlign="Center" Width="20%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TIPO_HORIENTACION" HeaderText="Horientacion" Visible="false">
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
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>



 <cc1:ModalPopupExtender ID="Mpe_Busqueda_Peritos_Externos" runat="server" 
                    TargetControlID="Btn_Comodin_Open_Busqueda_Peritos_Externos"
                    PopupControlID="Pnl_Busqueda_Contenedor" 
                    BackgroundCssClass="popUpStyle"
                    BehaviorID="Busqueda_Peritos_Externos"
                    CancelControlID="Btn_Comodin_Close_Busqueda_Peritos_Externos" 
                    DropShadow="true" 
                    DynamicServicePath="" 
                    Enabled="True" />
                <asp:Button ID="Btn_Comodin_Close_Busqueda_Peritos_Externos" runat="server"
                    Style="background-color: transparent; border-style:none;display:none;"  Text="" />
                <asp:Button  ID="Btn_Comodin_Open_Busqueda_Peritos_Externos" runat="server"
                    Style="background-color:transparent; border-style:none;display:none;" Text="" />

    <%--Ventana modal de búsqueda de peritos Externos--%>
    <asp:Panel ID="Pnl_Busqueda_Contenedor" runat="server" CssClass="drag"  HorizontalAlign="Center" Width="650px" 
    style="display:none;border-style:outset;border-color:Silver;background-image:url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG);background-repeat:repeat-y;">                         
    <asp:Panel ID="Panel1" runat="server" 
        style="cursor: move;background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
        <table width="99%">
            <tr>
                <td style="color:Black;font-size:12;font-weight:bold;">
                   <asp:Image ID="Img_Informatcion_Autorizacion" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                     B&uacute;squeda: Peritos Externos
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
                                          Nombre Perito Externo
                                        </td>              
                                        <td style="width:30%;text-align:right;font-size:11px;" colspan="3">
                                           <asp:TextBox ID="Txt_Busqueda_Nombre" runat="server" Width="98%"/>
                                        </td> 
                                        </tr>
                                        <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                          Usuario
                                        </td>              
                                        <td style="width:30%;text-align:right;font-size:11px;" colspan="3">
                                           <asp:TextBox ID="Txt_Usuario" runat="server" Width="98%"/>
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

                                                    <asp:BoundField DataField="PERITO_EXTERNO_ID" HeaderStyle-Width="15%" HeaderText="ID" Visible="false">
                                                    <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                    </asp:BoundField>

                                                    <asp:BoundField DataField="APELLIDO_PATERNO" HeaderStyle-Width="15%" HeaderText="Aellido Paterno" >
                                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                    </asp:BoundField>
                                                    
                                                    <asp:BoundField DataField="APELLIDO_MATERNO" HeaderStyle-Width="15%" HeaderText="Apellido Materno">
                                                    <HeaderStyle HorizontalAlign="Left" Width="30%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="30%" />
                                                    </asp:BoundField>
                                                    
                                                    <asp:BoundField DataField="NOMBRE" HeaderText="Nombre">
                                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                    </asp:BoundField>
                                                    
                                                    <asp:BoundField DataField="USUARIO" HeaderText="Usuario">
                                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
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
                                                 <asp:Button ID="Btn_Busqueda_Empleados" runat="server"  Text="Busqueda Peritos Externo" CssClass="button"  
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

   
    </asp:Content>                                                           
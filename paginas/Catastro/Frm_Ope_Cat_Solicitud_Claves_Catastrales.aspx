<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Ope_Cat_Solicitud_Claves_Catastrales.aspx.cs" Inherits="paginas_Catastro_Frm_Ope_Cat_Solicitud_Claves_Catastrales" 
MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"%>
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
            padding:0 5px 0 5px;
        }
        a.enlace_fotografia:hover
        {
            color: #25406D;
            text-decoration: underline;
            font-weight: bold;
            padding:0 5px 0 5px;
        }
        .style1
        {
            width: 239px;
        }
        .style2
        {
            width: 39%;
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
                        <td class="label_titulo" >
                             Solicitud Claves Catastrales
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
                                            <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button" TabIndex="1"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" OnClick="Btn_Nuevo_Click" AlternateText="Nuevo"/>
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Salir_Click" AlternateText="Salir" TabIndex="2"/>
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
                            <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                            <tr>
                        <td style="text-align:left;width:20%;">
                            <asp:HiddenField ID="Hdf_No_Clav_Catastral" runat="server" />
                            <asp:HiddenField ID="Hdf_Anio" runat="server" />
                        </td>
                        </tr>
                        <tr style="background-color: #3366CC">
                                <td style="text-align:left; font-size:15px; color:#FFFFFF;" colspan="4" >
                                    Datos de Claves Catastrales
                                </td>
                        </tr>
                        <tr>
                        <td style="width:20%">
                        *Cuenta Predial
                    </td>
                    <td style="width:30%">
                        <asp:TextBox ID="Txt_Cuenta_Predial" runat="server" Width="80%" 
                            TabIndex="3" Enabled="false" style="text-transform:uppercase"></asp:TextBox>
                        <asp:ImageButton ID="Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial" 
                            runat="server" Height="24px" 
                            ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" 
                            onclick="Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial_Click" TabIndex="10" 
                            ToolTip="Búsqueda Avanzada de Cuenta Predial" Width="24px"/>
                        <asp:HiddenField ID="Hdf_Cuenta_Predial_Id" runat="server" />
                    </td>
                    <td style="width:20%">
                        *Estatus
                    </td>
                        <td style="width:30%">
                            <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="98%">
                                <asp:ListItem Text="VIGENTE" Value="VIGENTE" />
                                <asp:ListItem Text="AUTORIZADA" Value="AUTORIZADA" />
                                <asp:ListItem Text="RECHAZADA" Value="RECHAZADA" />
                                <asp:ListItem Text="PAGADA" Value="PAGADA" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            *Solicitante
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Solicitante" runat="server" Width="98%" 
                                TabIndex="4" Enabled="false" MaxLength="50">
                            </asp:TextBox>
                        </td>
                        <td style="text-align:left;width:20%;">
                            *Tipo
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Tipo" runat="server" Width="98%" AutoPostBack="true" 
                                OnSelectedIndexChanged="Cmb_Tipo_SelectedIndexChanged" TabIndex ="5">
                                <asp:ListItem Text="<SELECCIONE>" Value="SELECCCIONE"/>
                                <asp:ListItem Text="ACC" Value="ACC"/>
                                <asp:ListItem Text="MCC" Value="MCC"/>
                                <asp:ListItem Text="AC" Value="AC"/>
                                <asp:ListItem Text="MC" Value="MC"/>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            *Correo
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Correo" runat="server" Width="98%" 
                                TabIndex="6" Enabled="false" MaxLength="50">
                            </asp:TextBox>
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
                            <tr ID="Tr_Fila_Fotografias_Bien" runat="server" >
                                <td style="text-align: left;width:20% " class="style1">
                                    *Nombre del Documento
                                </td>
                                <td style="text-align: left; width:30%" class="style1" >
                                    <asp:DropDownList ID="Cmb_Documento" runat="server" Width="98%" TabIndex="7"/>
                                </td>
                                <td style="text-align: left; width:20%" class="style2">
                                    *Documento
                                </td>
                                <td style="width:30%" class="style2">
                                    <asp:FileUpload ID="Fup_Documento" runat="server" Width="98%" TabIndex="8" />
                                </td>
                            </tr>
                            <tr>
                            <td></td>
                            </tr>
                            <tr>
                                <td style="width:20%"></td>
                                <td style="width:30%"></td>
                                <td style="width:20%"></td>
                                <td style="width:30%">
                                    <asp:ImageButton ID="Btn_Agregar_Documento" runat="server" Height="20px" 
                                        ImageUrl="~/paginas/imagenes/paginas/sias_add.png" 
                                        OnClick="Btn_Agregar_Documento_Click" ToolTip="Agregar Documento" Width="20px" TabIndex="17" />
                                 </td>
                            </tr>
                            <tr>
                                <td colspan="4" style="text-align: left; width: 20%;">
                                    <asp:GridView ID="Grid_Documentos" runat="server" AllowPaging="False" 
                                        AllowSorting="True" AutoGenerateColumns="False" HeaderStyle-CssClass="tblHead" 
                                        OnSelectedIndexChanged="Grid_Documentos_SelectedIndexChanged" PageSize="20" 
                                        OnDataBound="Grid_Documentos_DataBound" Style="white-space: normal;" Width="100%">
                                        <Columns>
                                            <asp:BoundField DataField="NO_DOCUMENTO" HeaderText="No documento" Visible="false">
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ANIO_DOCUMENTO" HeaderText="Anio documento" Visible="false">
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CLAVES_CATASTRALES_ID" HeaderText="Perito Id" Visible="false">
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="DOCUMENTO" HeaderText="Nombre Documento">
                                                <ItemStyle HorizontalAlign="Left" Width="40%"/>
                                            </asp:BoundField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                </ItemTemplate>
                                                <HeaderStyle Width="50%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="Btn_Eliminar_Fotos" runat="server" CommandName="Select" 
                                                        Height="20px" ImageUrl="~/paginas/imagenes/paginas/delete.png" 
                                                        ToolTip="Eliminar" Width="20px" />
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
                                
                                </td>
                            </tr>
                    </table>
                    <asp:HiddenField ID="Hdf_Clave_Externo_Id" runat="server" />
                    <asp:HiddenField ID="Hdf_Estatus_Clave_Catastral" runat="server" />
                </div>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>    
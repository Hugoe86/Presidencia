<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Ope_Cat_Solicitud_Refrendo.aspx.cs" MasterPageFile="~/paginas/Paginas_Generales/MasterPage_Ventanilla.master" Inherits="paginas_Catastro_Frm_Ope_Cat_Solicitud_Refrendo" %>
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
    </style>
    <!--SCRIPT PARA LA VALIDACION QUE NO EXPIRE LA SESSION-->

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
    <cc1:ToolkitScriptManager ID="Tool_ScriptManager" runat="server" EnableScriptGlobalization="true">
    </cc1:ToolkitScriptManager>
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel"
                DisplayAfter="0">
                <ProgressTemplate>
                    <%--<div id="progressBackgroundFilter" class="progressBackgroundFilter">
                    </div>
                    <div class="processMessage" id="div_progress">
                        <img alt="" src="../Imagenes/paginas/Updating.gif" /></div>--%>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Contenido" style="background-color: #ffffff; width: 100%; height: 100%;">
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo">
                            Solicitud de Refrendo
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
                                                ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" OnClick="Btn_Nuevo_Click" AlternateText="Nuevo"/>
                                            <%--<asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" OnClick="Btn_Modificar_Click" AlternateText="Modificar"/>--%>
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Salir_Click" AlternateText="Salir"/>
                                        </td>
                                        <td align="right" style="width: 41%;">
                                            <table style="width: 100%; height: 28px;">
                                                <tr>
                                                    <td style="vertical-align: middle; text-align: right; width: 20%;">
                                                        B&uacute;squeda:
                                                    </td>
                                                    <td style="width: 55%;">
                                                        <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100" ToolTip="Buscar" Width="180px" />
                                                        <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                                                            WatermarkText="<Folio>" TargetControlID="Txt_Busqueda" />
                                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" runat="server" TargetControlID="Txt_Busqueda"
                                                            FilterType="Numbers"/>
                                                    </td>
                                                    <td style="vertical-align: middle; width: 5%;">
                                                        <asp:ImageButton ID="Btn_Buscar" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                                            ToolTip="Buscar" />
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
                <div id="Div_Grid_Datos_Peritos" runat="server" visible="true">
                            <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                            <tr>
                        <td style="text-align:left;width:20%;">
                            <asp:HiddenField ID="Hdf_Perito_Externo" runat="server" />
                            <asp:HiddenField ID="Hdf_Solicitud_Id" runat="server" />
                        </td>
                        </tr>
                        <tr style="background-color: #3366CC">
                                <td style="text-align:left; font-size:15px; color:#FFFFFF;" colspan="4" >
                                    Datos del Perito Externo
                                </td>
                            </tr>
                      <tr>
                        <td style="text-align:left;width:20%;">
                            *Nombre
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Nombre" runat="server" Width="98%" 
                                TabIndex="9" Enabled="false" MaxLength="50" style="text-transform:uppercase"></asp:TextBox>
                        </td>
                        <td style="text-align:left;width:20%;text-align:right;">
                            *Apellido Paterno
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Apellido_Paterno" runat="server" Width="96.4%" TabIndex="11" Enabled="false"  MaxLength="50" style="text-transform:uppercase"/>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            *Apellido Materno
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Apellido_Materno" runat="server" Width="98%" 
                                TabIndex="9" Enabled="false"  MaxLength="50" style="text-transform:uppercase"></asp:TextBox>
                        </td>
                        <td style="text-align:right;width:20%; vertical-align:top;">
                            *Correo Electronico
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_E_Mail" runat="server" TabIndex="13"
                                Width="98%" Enabled="false"/>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            *Calle
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Calle" runat="server" Width="98%" 
                                TabIndex="9" Enabled="false"  MaxLength="50" style="text-transform:uppercase"></asp:TextBox>
                        </td>
                        <td style="text-align:left;width:20%;text-align:right;">
                            *Colonia
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Colonia" runat="server" Width="98%" TabIndex="11" Enabled="false"  MaxLength="50" style="text-transform:uppercase"/>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;text-align:left;">
                            *Estado
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Estado" runat="server" Width="98%" TabIndex="11" Enabled="false"  MaxLength="20" style="text-transform:uppercase"/>
                        </td>
                        <td style="text-align:left;width:20%;text-align:right;">
                            *Ciudad
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Ciudad" runat="server" Width="98%" TabIndex="11" Enabled="false"  MaxLength="20" style="text-transform:uppercase"/>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%; vertical-align:top;">
                            *Teléfono
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Telefono" runat="server" TabIndex="13" MaxLength="10"
                                Width="98%" style="text-transform:uppercase;"  Enabled="false"/>
                                <cc1:FilteredTextBoxExtender FilterType="Numbers" runat="server" ID="FTB_Txt_Anio" TargetControlID="Txt_Telefono" />
                        </td>
                        <td style="text-align:right;width:20%; vertical-align:top;">
                            Celular
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Celular" runat="server" TabIndex="13" MaxLength="10"
                                Width="98%" style="text-transform:uppercase;"  Enabled="false"/>
                                <cc1:FilteredTextBoxExtender FilterType="Numbers" runat="server" ID="FilteredTextBoxExtender1" TargetControlID="Txt_Celular" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%; vertical-align:top;">
                            *Estatus
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="98%" TabIndex="12" Enabled="false">
                                <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE"></asp:ListItem>
                                <asp:ListItem Text="VIGENTE" Value="VIGENTE"></asp:ListItem>
                                <asp:ListItem Text="VALIDAR" Value="VALIDAR"></asp:ListItem>
                                <asp:ListItem Text="POR PAGAR" Value="POR PAGAR"></asp:ListItem>
                                <asp:ListItem Text="PAGADO" Value="PAGADO"></asp:ListItem>
                                <asp:ListItem Text="VALIDADO" Value="VALIDADO"></asp:ListItem>
                                 <asp:ListItem Text="REFRENDAR" Value="REFRENDAR"></asp:ListItem>
                                <asp:ListItem Text="BAJA" Value="BAJA"></asp:ListItem> 
                            </asp:DropDownList>
                        </td>
                        <td style="text-align:right;width:20%; vertical-align:top;">
                            *Fecha Vencimiento
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Fecha" runat="server" TabIndex="13"
                                Width="98%" Enabled="false"/>
                        </td>
                    </tr>

                    <tr>
                        <td style="text-align:left;width:20%; vertical-align:top;">
                            Información adicional
                        </td>
                        <td colspan="3" style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Informacion_Adicional" runat="server" TabIndex="13" MaxLength="250"
                                Width="98%" style="text-transform:uppercase;"  Enabled="false" Rows="3" TextMode="MultiLine"/>
                        </td>
                    </tr>
                    <tr>
                    <td>
                    <br />
                    <br />
                    </td>
                    </tr>
                    <tr>
                    <td>
                    <br />
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
                            <tr ID="Tr_Fila_Fotografias_Bien" runat="server">
                                <td style="text-align: left; width: 20%;">
                                    Documento
                                </td>
                                <td style="text-align: left; width: 30%;">
                                    <asp:FileUpload ID="Fup_Documento" runat="server" Width="235px" />
                                </td>
                                <td colspan="2" style="text-align: left;">
                                    <%--<asp:ImageButton ID="Btn_Subir_Archivo" runat="server" 
                                        ImageUrl="~/paginas/imagenes/paginas/sias_upload.png" 
                                        OnClick="Btn_Subir_Archivo_Click" Style="border: 0 none;
                                    width: 20px; height: 20px; padding: 0;" ToolTip="Enviar archivo" />--%>
                                    Nombre Documento
                                    <asp:TextBox ID="Txt_Nombre_Documento" runat="server" Enabled="false" Width="80%" />
                                    &nbsp; &nbsp;
                                    <asp:ImageButton ID="Btn_Agregar_Documento" runat="server" Height="20px" 
                                        ImageUrl="~/paginas/imagenes/paginas/sias_add.png" 
                                        OnClick="Btn_Agregar_Documento_Click" ToolTip="Agregar Documento" Width="20px" />
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
                                            <asp:BoundField DataField="PERITO_EXTERNO_ID" HeaderText="Perito Id" Visible="false">
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
                                            <asp:BoundField DataField="NO_BIEN" Visible="false" />
                                        </Columns>
                                        <HeaderStyle CssClass="tblHead" />
                                    </asp:GridView>
                                </td>
                            </tr>

                        <%--<tr style="background-color: #36C;">
                                <td colspan="4" style="text-align: left; font-size: 15px; color: #FFF;">
                                    Observaciones al Perito
                                </td>
                            </tr>
                        <tr>
                            <td style="text-align: left; width: 20%; vertical-align: top;">
                                Observaciones
                            </td>
                            <td colspan="3" style="text-align: left; width: 80%; vertical-align: top;">
                                <asp:TextBox ID="Txt_Observaciones" runat="server" MaxLength="250" 
                                    Style="text-transform: uppercase;" TextMode="MultiLine" Width="98.6%" Enabled="false"/>
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Observaciones" runat="server" 
                                    TargetControlID="Txt_Observaciones" WatermarkCssClass="watermarked" 
                                    WatermarkText="Límite de Caracteres 250" />
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Observaciones" runat="server" 
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                    TargetControlID="Txt_Observaciones" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " />
                            </td>
                            </tr>--%>
                    </table>
                </div>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Cat_Cat_Seguimiento_Solicitud_Registro.aspx.cs" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" Inherits="paginas_Catastro_Frm_Cat_Cat_Seguimiento_Solicitud_Registro" %>
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
    function ValidarCaracteres(textareaControl, maxlength) { if (textareaControl.value.length > maxlength) { textareaControl.value = textareaControl.value.substring(0, maxlength); alert("Debe ingresar hasta un máximo de " + maxlength + " caracteres"); } }

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
                        <img alt="" src="../Imagenes/paginas/Updating.gif" />
                    </div>--%>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Contenido" style="background-color: #ffffff; width: 100%; height: 100%;">
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo" >
                            Seguimiento: Solicitud de Registro
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
                </td>
                </tr>
                    <tr align="center">
                        <td>
                            <div style="width: 99%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold;
                                font-style: normal; font-variant: normal; font-family: fantasy; height: 32px">
                                <table style="width: 100%; height: 28px;">
                                    <tr>
                                        <td align="left" style="width: 59%;">
                                            <%--<asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" OnClick="Btn_Modificar_Click" AlternateText="Modificar"/>--%>
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Salir_Click" AlternateText="Salir"/>
                                             <asp:ImageButton ID="Btn_Aceptar" runat="server" ToolTip="Aceptar Solicitud" CssClass="Img_Button"
                                                ImageUrl="~/paginas/imagenes/paginas/accept.png" OnClick="Btn_Aceptar_Click" 
                            AlternateText="Salir"/> 
                             <asp:ImageButton ID="Btn_Rechazar" runat="server" ToolTip="Rechazar Solicitud" CssClass="Img_Button"
                                                ImageUrl="~/paginas/imagenes/paginas/delete.png" OnClick="Btn_Cancelar_Click" 
                            AlternateText="Salir" Height="24px"/>     
                                        
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
                                                            WatermarkText="NOMBRE SOLICITANTE" TargetControlID="Txt_Busqueda" />
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
                <table  width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                <div id="Div_Grid_Peritos" runat="server">
                    <tr style="background-color: #3366CC">
                                <td style="text-align:left; font-size:15px; color:#FFFFFF;" colspan="4" >
                                    Peritos Externos Solicitantes
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="4">
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
                                                    <asp:BoundField DataField="TEMP_PERITO_EXTERNO_ID" HeaderStyle-Width="15%" HeaderText="ID" Visible="false">
                                                    <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                    </asp:BoundField>
                                                    
                                                    <asp:BoundField DataField="PERITO_EXTERNO" HeaderStyle-Width="15%" HeaderText="Perito Externo">
                                                    <HeaderStyle HorizontalAlign="Left" Width="70%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="70%" />
                                                    </asp:BoundField>
                                                    
                                                    <asp:BoundField DataField="E_MAIL" HeaderText="E-Mail">
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
                                            <br />
                                </td>
                            </tr>
                            </div>
                            </table>
                            <asp:HiddenField ID="Hdf_Perito_Externo_Id" runat="server" />
                            <asp:HiddenField ID="Hdf_Solicitud_Id" runat="server" />
                            <asp:HiddenField ID="Hdf_Estatus_Perito_Externo" runat="server" />
                            <br />
                <div id="Div_Grid_Datos_Peritos" runat="server" visible="false">
                            <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                            <tr>
                        <td style="text-align:left;width:20%;">
                        </td>
                        </tr>
                        <tr style="background-color: #3366CC">
                                <td style="text-align:left; font-size:15px; color:#FFFFFF;" colspan="4" >
                                    Datos del Perito Externo Solicitante
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
                        <td style="text-align:left;width:20%;">
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
                        <td style="text-align:left;width:20%;">
                            *Correo Electronico <%-- Cambios --%>
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
                        <td style="text-align:left;width:20%;">
                            *Colonia <%-- Cambios --%>
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
                        <td style="text-align:left;width:20%;">
                            *Ciudad<%-- Cambios --%>
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
                        <td style="text-align:left;width:20%; ">
                            Celular<%-- Cambios --%>
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Celular" runat="server" TabIndex="13" MaxLength="10"
                                Width="98%" style="text-transform:uppercase;"  Enabled="false"/>
                                <cc1:FilteredTextBoxExtender FilterType="Numbers" runat="server" ID="FilteredTextBoxExtender1" TargetControlID="Txt_Celular" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
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
                                <asp:ListItem Text="BAJA" Value="BAJA"></asp:ListItem> 
                            </asp:DropDownList>
                        </td>
                        <td style="text-align:left;width:20%; ">
                            *Fecha Vencimiento <%-- Cambios --%>
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Fecha" runat="server" TabIndex="13"
                                Width="98%" Enabled="false"/>
                        </td>
                    </tr>
                    <%-- Cambios --%>
                    <tr>
                        <td style="text-align:left;width:20%; vertical-align:top;">
                            RFC
                        </td>
                        <td colspan="3" style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Informacion_Adicional" runat="server" TabIndex="14" MaxLength="15"
                                Width="37%" style="text-transform:uppercase;"  Enabled="false" />
                        </td>
                    </tr>
                   <%-- Cambios --%>
                    <tr>
                        <td style="text-align:left;width:20%; vertical-align:top;">
                            Información adicional
                        </td>
                        <td colspan="3" style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Informacion" runat="server" TabIndex="15" MaxLength="250"
                                Width="98%" style="text-transform:uppercase;"  Enabled="false" Rows="3" TextMode="MultiLine"/>
                        </td>
                    </tr>
                    <tr>
                    <td>
                    <br />
                    </td>
                    </tr>
                    </table>
                    </div>
                <div id="Div_Detalles" runat="server" visible="false">
                    <table width="98%" class="estilo_fuente">
                            <tr style="background-color: #36C;">
                                <td colspan="4" style="text-align: left; font-size: 15px; color: #FFF;">
                                    Documentos
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" style="text-align: left; width: 20%;">
                                    <asp:GridView ID="Grid_Documentos" runat="server" AllowPaging="False" 
                                        AllowSorting="True" AutoGenerateColumns="False" HeaderStyle-CssClass="tblHead" PageSize="20" 
                                        OnDataBound="Grid_Documentos_DataBound" Style="white-space: normal;" Width="100%">
                                        <Columns>
                                            <asp:BoundField DataField="NO_DOCUMENTO" HeaderText="No documento" Visible="false">
                                            <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="TEMP_PERITO_EXTERNO_ID" HeaderText="Perito Id" Visible="false">
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
                                        </Columns>
                                        <HeaderStyle CssClass="tblHead" />
                                    </asp:GridView>
                                </td>
                            </tr>

                        <tr style="background-color: #36C;">
                                <td colspan="4" style="text-align: left; font-size: 15px; color: #FFF;">
                                    Observaciones para enviar al correo del perito externo
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
                            </tr>
                    </table>
                </div>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
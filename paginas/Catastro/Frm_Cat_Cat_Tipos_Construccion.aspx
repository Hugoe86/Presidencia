﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Cat_Cat_Tipos_Construccion.aspx.cs" Inherits="paginas_Catastro_Frm_Cat_Cat_Tipos_Construccion" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" Title="Catálogo de Tipos de Construcción"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">
    <script type='text/javascript'>
    function ValidarCaracteres(textareaControl, maxlength) { if (textareaControl.value.length > maxlength) { textareaControl.value = textareaControl.value.substring(0, maxlength); alert("Debe ingresar hasta un máximo de " + maxlength + " caracteres"); } }

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
                            Catálogo de Tipos de Construcción
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
                                            <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button"
                                                TabIndex="1" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" AlternateText="Nuevo"
                                                OnClick="Btn_Nuevo_Click" />
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
                            *Tipo
                        </td>
                        <td colspan="3" style="width: 82%; text-align: left;">
                            <asp:TextBox TextMode="MultiLine" runat="server" ID="Txt_Tipo" Rows="2" Width="96.4%"
                                Style="float: left; text-transform: uppercase" />
                            <asp:HiddenField ID="Hdf_Tipo_Construccion_Id" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            *Estatus
                        </td>
                        <td style="width: 30%;text-align: left;">
                            <asp:DropDownList ID="Cmb_Estatus" runat="server">
                            <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE"/>
                            <asp:ListItem Text="VIGENTE" Value="VIGENTE"/>
                            <asp:ListItem Text="BAJA" Value="BAJA"/>
                            </asp:DropDownList>
                        </td>
                        <td style="text-align: left; width: 20%;">
                        </td>
                        <td style="width: 30%; text-align:left;">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <table width="98%" class="estilo_fuente">
                                <tr>
                                    <td style="text-align: left;">
                                        <asp:GridView ID="Grid_Tipo_Construccion" runat="server" AllowSorting="true" AutoGenerateColumns="False"
                                            CssClass="GridView_1" HeaderStyle-CssClass="tblHead" Style="white-space: normal;"
                                            Width="100%" OnPageIndexChanging="Grid_Tipo_Construccion_PageIndexChanging" OnSelectedIndexChanged="Grid_Tipo_Construccion_SelectedIndexChanged"
                                            PageSize="10" AllowPaging="true">
                                            <Columns>
                                                <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/paginas/Select_Grid_Inner.png"
                                                    Text="Button">
                                                    <HeaderStyle Width="5%" />
                                                </asp:ButtonField>
                                                <asp:BoundField DataField="TIPO_CONSTRUCCION_ID" HeaderText="Id" Visible="false">
                                                    <ItemStyle HorizontalAlign="Center"/>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="IDENTIFICADOR" HeaderText="Tipo Construcción">
                                                    <ItemStyle HorizontalAlign="Left"  Width="80%"/>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
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
</asp:Content>
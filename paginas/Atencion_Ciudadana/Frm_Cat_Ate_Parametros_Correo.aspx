<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    CodeFile="Frm_Cat_Ate_Parametros_Correo.aspx.cs" Inherits="paginas_Atencion_Ciudadana_Frm_Cat_Ate_Parametros_Correo"
    ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">

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
    function pageLoad() {
        Contar_Caracteres();
    }

    function Contar_Caracteres() {
        $('textarea[id$=Txt_Descripcion]').keyup(function() {
            var Caracteres = $(this).val().length;

            if (Caracteres > 1000) {
                this.value = this.value.substring(0, 1000);
                $(this).css("background-color", "Yellow");
                $(this).css("color", "Red");
            } else {
                $(this).css("background-color", "White");
                $(this).css("color", "Black");
            }

            $('#Contador_Caracteres_Comentarios').text('Carácteres Ingresados [ ' + Caracteres + ' ]/[ 1000 ]');
        });
    }    
            
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <cc1:ToolkitScriptManager ID="Tsm_Acciones" runat="server" AsyncPostBackTimeout="3600"
        EnableScriptGlobalization="true" EnableScriptLocalization="true" />
    <div id="Div_Principal" style="background-color: #ffffff; width: 100%; height: 100%;">
        <asp:UpdatePanel ID="Upd_Panel" runat="server">
            <ContentTemplate>
                <asp:UpdateProgress ID="Uprg_Servicios" runat="server" AssociatedUpdatePanelID="Upd_Panel"
                    DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                        </div>
                        <div class="processMessage" id="div_progress">
                            <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <table width="99.5%" border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo" colspan="4">
                            Parámetros para envío de correos
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Image ID="Img_Informacion" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" />
                            <asp:Label ID="Lbl_Informacion" runat="server" ForeColor="Red" Text="Mensajes de advertencia"></asp:Label>
                        </td>
                    </tr>
                </table>
                <div id="Div_Barra_Herramientas" runat="server">
                    <table width="99.5%" border="0" cellspacing="0" class="estilo_fuente">
                        <tr class="barra_busqueda">
                            <td colspan="2">
                                <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                    CssClass="Img_Button" AlternateText="Modificar" ToolTip="Modificar" OnClick="Btn_Modificar_Click" />
                                <asp:ImageButton ID="Btn_Salir" runat="server" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                    ToolTip="Inicio" OnClick="Btn_Salir_Click" />
                            </td>
                            <td colspan="2" align="right">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="Div_Contenido1" runat="server" style="width: 99%;">
                    <table width="99.5%" border="0" cellspacing="0">
                        <tr>
                            <td colspan="4">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%;">
                                Tipo de correo
                            </td>
                            <td style="width: 30%;">
                                <asp:DropDownList ID="Cmb_Tipo_Correo" runat="server" Width="100%" OnSelectedIndexChanged="Cmb_Tipo_Correo_SelectedIndexChanged"
                                    AutoPostBack="true">
                                    <asp:ListItem Value="NOTIFICACION">Notificación</asp:ListItem>
                                    <asp:ListItem Value="FELICITACION">Felicitación</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%;">
                                Servidor de correo
                            </td>
                            <td style="width: 30%;">
                                <asp:TextBox ID="Txt_Servidor" runat="server" MaxLength="256" Style="width: 98%;"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Servidor" runat="server" TargetControlID="Txt_Servidor"
                                    FilterType="LowercaseLetters, UppercaseLetters, Numbers, Custom" ValidChars="./">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                            <td style="width: 20%; text-align: right;">
                                Puerto
                            </td>
                            <td style="width: 30%;">
                                <asp:TextBox ID="Txt_Puerto" runat="server" MaxLength="8" Style="width: 98%;"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Puerto" runat="server" TargetControlID="Txt_Puerto"
                                    FilterType="Numbers">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%;">
                                Usuario
                            </td>
                            <td style="width: 30%;">
                                <asp:TextBox ID="Txt_Usuario" runat="server" MaxLength="256" Style="width: 98%;"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Usuario" runat="server" TargetControlID="Txt_Usuario"
                                    FilterType="LowercaseLetters, UppercaseLetters, Numbers, Custom" ValidChars="_-.@">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                            <td style="width: 20%; text-align: right;">
                                Contraseña
                            </td>
                            <td style="width: 30%;">
                                <asp:TextBox ID="Txt_Password" runat="server" MaxLength="50" TextMode="Password"
                                    Style="width: 98%;"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%; vertical-align: top;">
                                Asunto
                            </td>
                            <td style="width: 80%;" colspan="3">
                                <asp:TextBox ID="Txt_Saludo_Correo" runat="server" MaxLength="4000" Style="width: 99%;"
                                    TextMode="MultiLine"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Saludo_Correo" runat="server" TargetControlID="Txt_Saludo_Correo"
                                    FilterType="LowercaseLetters, UppercaseLetters, Numbers, Custom" ValidChars="ÑñáéíóúÁÉÍÓÚ,-./()<>:&; ">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%; vertical-align: top;">
                                Cuerpo
                            </td>
                            <td style="width: 80%;" colspan="3">
                                <asp:TextBox ID="Txt_Cuerpo_Correo" runat="server" Style="width: 99%; overflow: scroll;
                                    max-width: 665px;" TextMode="MultiLine" Height="150"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%; vertical-align: top;">
                                Firma
                            </td>
                            <td style="width: 80%;" colspan="3">
                                <asp:TextBox ID="Txt_Firma_Correo" runat="server" MaxLength="4000" Style="width: 99%;"
                                    TextMode="MultiLine"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Firma_Correo" runat="server" TargetControlID="Txt_Firma_Correo"
                                    FilterType="LowercaseLetters, UppercaseLetters, Numbers, Custom" ValidChars="ÑñáéíóúÁÉÍÓÚ.-,/<>:&; ">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <div id="Encabezado_Archivos" runat="server" class="barra_busqueda" style="vertical-align: middle;
                    width: 99.5%;">
                    <span style="position: relative; top: 5px;">&nbsp; Archivos</span>
                </div>
                <br />
                Subir archivo 
                <asp:FileUpload ID="Fup_Archivo" runat="server" size="101" style="width:740px;" />
                <asp:ImageButton ID="Btn_Subir_Archivo" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_add.png" ToolTip="Subir archivo" OnClick="Btn_Subir_Archivo_Click" />
                <br /><br />
                <div id="Contenedor_Archivos" runat="server">
                    <asp:GridView ID="Grid_Archivos" runat="server" AutoGenerateColumns="False" CssClass="GridView_1"
                        Width="99.5%" GridLines="None">
                        <RowStyle CssClass="GridItem" />
                        <Columns>
                            <asp:BoundField DataField="Nombre_Archivo" HeaderText="Archivo" Visible="True" />
                            <asp:BoundField DataField="Cadena_Archivo" HeaderText="Identificador" Visible="True" />
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <asp:ImageButton ID="Btn_Eliminar" OnClientClick="javascript: return confirm('El archivo será eliminado del servidor ¿Confirma que desea continuar?');"
                                        runat="server" ImageUrl="~/paginas/imagenes/paginas/quitar.png" OnClick="Btn_Eliminar_Click"
                                        CommandArgument='<%# Eval("NOMBRE_ARCHIVO") %>' ToolTip="Eliminar archivo" />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" Width="35px" />
                                <ItemStyle HorizontalAlign="Center" Width="35px" />
                            </asp:TemplateField>
                        </Columns>
                        <SelectedRowStyle CssClass="GridSelected" />
                        <PagerStyle CssClass="GridHeader" />
                        <HeaderStyle CssClass="GridHeader" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                    </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

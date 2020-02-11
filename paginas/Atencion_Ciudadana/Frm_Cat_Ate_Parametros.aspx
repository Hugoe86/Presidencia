<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    CodeFile="Frm_Cat_Ate_Parametros.aspx.cs" Inherits="paginas_Atencion_Ciudadana_Frm_Cat_Ate_Parametros" %>

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
                            Parámetros de Atenci&oacute;n Ciudadana
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Image ID="Img_Informacion" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" />
                            <asp:Label ID="Lbl_Informacion" runat="server" ForeColor="Red" TabIndex="0" Text="Mensajes de advertencia"></asp:Label>
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
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 30%;">
                                *&nbsp;Origen por defecto para ventanilla
                            </td>
                            <td style="width: 70%;">
                                <asp:DropDownList ID="Cmb_Programa_Atencion" runat="server" Width="100%">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 30%;">
                                *&nbsp;Origen para peticiones de ventanilla Web
                            </td>
                            <td style="width: 70%;">
                                <asp:DropDownList ID="Cmb_Programa_Ventanilla_Web" runat="server" Width="100%">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 30%;">
                                *&nbsp;Origen que genera Formato Consecutivo
                            </td>
                            <td style="width: 70%;">
                                <asp:DropDownList ID="Cmb_Programa_Formato_Consecutivo" runat="server" Width="100%">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Pre_Parametros_Cajas.aspx.cs" Inherits="paginas_Predial_Frm_Ope_Pre_Parametros_Cajas" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">

    <script language="javascript" type="text/javascript">
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
        setInterval('MantenSesion()', "<%=(int)(0.9*(Session.Timeout * 60000))%>");
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel_Padron_Predios" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Padrones" runat="server" AssociatedUpdatePanelID="Upd_Panel_Padron_Predios"
                DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                    </div>
                    <div class="processMessage" id="div_progress">
                        <img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="General" style="background-color: #ffffff; width: 100%; height: 100%;">
                <table id="Tbl_Comandos" border="0" cellspacing="0" class="estilo_fuente" style="width: 98%;">
                    <tr>
                        <td class="label_titulo" colspan="2">
                            Parámetros Cajas
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div id="Div_Contenedor_Error" runat="server">
                                <tr>
                                    <asp:Image ID="Img_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" />
                                    <asp:Label ID="Lbl_Ecabezado_Mensaje" runat="server" CssClass="estilo_fuente_mensaje_error"
                                        Text="" />
                                    <caption>
                                        <br />
                                        <asp:Label ID="Lbl_Error" runat="server" CssClass="estilo_fuente_mensaje_error" TabIndex="0"
                                            Text=""></asp:Label>
                                    </caption>
                                </tr>
                            </div>
                        </td>
                    </tr>
            </div>
            </tr>
            <tr class="barra_busqueda">
                <td style="width: 50%">
                    <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                        CssClass="Img_Button" OnClick="Btn_Modificar_Click" />
                    <asp:ImageButton ID="Btn_Salir" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                        CssClass="Img_Button" OnClick="Btn_Salir_Click" />
                </td>
            </tr>
            </table>
            <table border="0" cellspacing="0" class="estilo_fuente" style="width: 98%;">
                <tr>
                    <td colspan="2">
                        Tolerancia Pagos Otras Instituciones
                    </td>
                    <td style="width: 18%">
                        &nbsp;
                    </td>
                    <td style="width: 32%">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="width: 18%">
                        *Limite superior
                    </td>
                    <td style="width: 32%">
                        <asp:TextBox ID="Txt_Tolerancia_Limite_Superior" Width="92%" runat="server"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="Txt_Tolerancia_Limite_Superior"
                            FilterType="Numbers, Custom" ValidChars="." />
                    </td>
                    <td style="width: 18%">
                    </td>
                    <td style="width: 32%">
                    </td>
                </tr>
                <tr>
                    <td style="width: 18%">
                        *Limite inferior
                    </td>
                    <td style="width: 32%">
                        <asp:TextBox ID="Txt_Tolerancia_Limite_Inferior" runat="server" Width="92%"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="Txt_Tolerancia_Limite_Inferior"
                            FilterType="Numbers, Custom" ValidChars="." />
                    </td>
                    <td style="width: 18%">
                        &nbsp;
                    </td>
                    <td style="width: 32%">
                        &nbsp;
                    </td>
                </tr>
            </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

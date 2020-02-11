<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" CodeFile="Frm_Ope_Psp_Actualizar_Presupuesto_2013.aspx.cs" Inherits="paginas_Presupuestos_Frm_Ope_Psp_Actualizar_Presupuesto_2013" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
   <!--SCRIPT PARA LA VALIDACION QUE NO EXPERE LA SESSION-->  
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
        setInterval("MantenSesion()", <%=(int)(0.9*(Session.Timeout * 1000000))%>);
        
    //-->
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <cc1:ToolkitScriptManager ID="ScriptManager2" runat="server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true">
    </cc1:ToolkitScriptManager>
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
                               Actualización de Presupuestos
                            </td>
                        </tr>
                        <tr align="left">
                            <td colspan="4">
                                <asp:Image ID="Img_Warning" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png"
                                    Visible="false" />
                                <asp:Label ID="Lbl_Informacion" runat="server" ForeColor="#990000"></asp:Label>
                            </td>
                        </tr>
                        <tr class="barra_busqueda" align="right">
                            <td align="left" valign="middle" colspan="2">
                                <asp:ImageButton ID="Btn_Salir" runat="server" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                    ToolTip="Inicio" OnClick="Btn_Salir_Click" />
                            </td>
                            <td colspan="2">
                            </td>
                        </tr>
                    </table>
                </div>
                 <%--Div Contenido--%>
                <div id="Div_Contenido" runat="server">
                    <table style="width: 100%;">
                        <tr>
                            <td style="width:45%">
                                <cc1:AsyncFileUpload ID="FileUp" runat="server" ErrorBackColor="Red" CompleteBackColor="Lime"
                                    UploadingBackColor="Silver"  />
                            </td>
                            <td align="left" style="text-align:left;">
                                <asp:ImageButton ID="Btn_Limpiar" runat="server" Width="20px" ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png"
                                    ToolTip="Limpiar" />
                            </td>
                            <td style="text-align:right;">
                                <asp:Button ID="Btn_Subir_Archivo" runat="server" Text="Subir Archivo" OnClick="Btn_Subir_Archivo_Click"
                                    CssClass="button" />
                            </td>
                            <td>
                                <asp:Button ID="Btn_Actualizar_Presupuesto" runat="server" Text="Actualizar Presupuesto"
                                    OnClick="Btn_Actualizar_Presupuesto_Click" CssClass="button" Width="200px" />
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    
</asp:Content>
